package simbir.apex.service.agent.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.db.entity.Alert;
import simbir.apex.service.agent.model.EventDto;
import simbir.apex.service.agent.model.RuleDto;
import simbir.apex.service.agent.model.enums.Status;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.sql.Timestamp;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class EventProcessor {

    private final RuleService ruleService;
    private final AlertService alertService;

    private final Map<String, Deque<EventDto>> eventHistory = new ConcurrentHashMap<>();
    private final ExecutorService executor = Executors.newFixedThreadPool(8);
    private final ObjectMapper objectMapper = new ObjectMapper();

    public void processEvent(EventDto event) {
        executor.submit(() -> {
            List<RuleDto> rules = ruleService.fetchRules();

            for (RuleDto rule : rules) {
                if (matchesRule(event, rule)) {
                    // Создаём историю событий для этого правила
                    List<EventDto> history = new ArrayList<>(
                            eventHistory.getOrDefault(rule.getName(), new ArrayDeque<>())
                    );

                    try {
                        // Сериализуем историю событий в JSON для rawData
                        String rawDataJson = objectMapper.writeValueAsString(history);

                        // Формируем алерт
                        Alert alert = Alert.builder()
                                .ruleId(rule.getId())
                                .assignedTo(null) // никто не взял в работу
                                .hostname(event.getDevice())
                                .title(rule.getName())
                                .description("Совпадение по правилу: " + rule.getLogic())
                                .status(Status.NEW)
                                .severity(null)
                                .rawData(rawDataJson)
                                .createdAt(new Timestamp(System.currentTimeMillis()))
                                .closedAt(null)
                                .resolutionNotes(null)
                                .build();

                        alertService.sendAlert(alert);

                    } catch (Exception e) {
                        System.err.println("Ошибка при создании Alert: " + e.getMessage());
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

            boolean baseMatch = (action == null || action.equals(event.getEventId()));
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
            System.err.println("Ошибка при проверке правила " + rule.getName() + ": " + e.getMessage());
            return false;
        }
    }
}
