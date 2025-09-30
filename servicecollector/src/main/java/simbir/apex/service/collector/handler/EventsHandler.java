package simbir.apex.service.collector.handler;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Component;
import simbir.apex.service.collector.config.KafkaConfiguration;

@Component
public class EventsHandler {
    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    @KafkaListener(topics = KafkaConfiguration.TOPIC)
    public void handle(String message) {
        logger.info("Received message: {}", message);
        // TODO: implement handling
    }
}
