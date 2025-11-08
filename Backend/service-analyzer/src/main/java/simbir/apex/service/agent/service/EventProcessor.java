package simbir.apex.service.agent.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.db.entity.Alert;
import simbir.apex.service.agent.db.entity.Event;
import simbir.apex.service.agent.model.EventDto;
import simbir.apex.service.agent.model.RuleDto;
import simbir.apex.service.agent.model.enums.Status;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.sql.Timestamp;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class EventProcessor {

    private final Logger logger = LoggerFactory.getLogger(EventProcessor.class);

    private final RuleService ruleService;
    private final AlertService alertService;
    private final EventService eventService;

    private Map<String, Deque<EventDto>> eventHistory = new ConcurrentHashMap<>();
    private ExecutorService executor = Executors.newFixedThreadPool(8);
    private ObjectMapper objectMapper = new ObjectMapper();

    public void processEvent(EventDto event) {
        executor.submit(() -> {

            Event savedEvent = Event.builder()
                    .isLan(event.isLan())
                    .device(event.getDevice())
                    .eventId(event.getEventId())
                    .severity(event.getSeverity())
                    .timestamp(new Date())
                    .build();
            eventService.saveEvent(savedEvent);

            List<RuleDto> rules = ruleService.fetchRules();

            for (RuleDto rule : rules) {
                if (matchesRule(event, rule)) {
                    List<EventDto> history = new ArrayList<>(
                            eventHistory.getOrDefault(rule.getName(), new ArrayDeque<>())
                    );

                    try {
                        logger.info("Trying to create new alert {}", rule.getName());

                        String rawDataJson = objectMapper.writeValueAsString(history);

                        Alert alert = Alert.builder()
                                .ruleId(rule.getId())
                                .assignedTo(null)
                                .hostname(event.getDevice())
                                .title(rule.getName())
                                .description("Совпадение по правилу: " + rule.getLogic())
                                .status(String.valueOf(Status.NEW))
                                .severity(event.getSeverity())
                                .rawData(rawDataJson)
                                .createdAt(new Timestamp(System.currentTimeMillis()))
                                .closedAt(null)
                                .resolutionNotes("Сработало правило: " + rule.getName() + ". Логика: " + rule.getLogic())
                                .build();

                        alertService.sendAlert(alert);

                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            }
        });
    }

    private boolean matchesRule(EventDto event, RuleDto rule) {
        try {
            String[] parts = rule.getLogic().split(";");
            Map<String, String> params = Arrays.stream(parts)
                    .map(s -> s.split("=", 2))
                    .filter(a -> a.length == 2)
                    .collect(Collectors.toMap(a -> a[0].trim(), a -> a[1].trim()));

            String action = params.get("action");
            String sequence = params.get("sequence");
            int count = Integer.parseInt(params.getOrDefault("count", "1"));

            boolean baseMatch = (action == null || action.equalsIgnoreCase(event.getEventId()));
            if (!baseMatch) return false;

            eventHistory.computeIfAbsent(rule.getName(), k -> new ArrayDeque<>()).add(event);
            Deque<EventDto> history = eventHistory.get(rule.getName());
            while (history.size() > count) history.pollFirst();

            if (sequence != null) {
                boolean hasSequence = history.stream().anyMatch(e -> sequence.equals(e.getEventId()));
                return hasSequence;
            }

            if (history.size() >= count) {
                boolean allSameAction = history.stream()
                        .allMatch(e -> Objects.equals(e.getEventId(), action));
                return allSameAction;
            }

            return false;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }
}

