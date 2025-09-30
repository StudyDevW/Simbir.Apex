package simbir.apex.service.collector.handler;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Component;
import simbir.apex.service.collector.config.KafkaConfiguration;
import simbir.apex.service.collector.service.EventCollectorService;

@Component
public class EventsHandler {
    private final Logger logger = LoggerFactory.getLogger(this.getClass());
    private final ObjectMapper objectMapper = new ObjectMapper();

    private final EventCollectorService eventCollectorService;

    public EventsHandler(EventCollectorService eventCollectorService) {
        this.eventCollectorService = eventCollectorService;
    }

    @KafkaListener(topics = KafkaConfiguration.TOPIC)
    public void handle(String message) {
        logger.info("Received message: {}", message);

        try {
            EventDto eventDto = objectMapper.readValue(message, EventDto.class);
            logger.info("Successfully parsed message");

            eventCollectorService.processEvent(eventDto);
        } catch (Exception e) {
            logger.error("Failed to parse message: {}", message, e);
            //throw new RuntimeException(e);
        }

    }
}
