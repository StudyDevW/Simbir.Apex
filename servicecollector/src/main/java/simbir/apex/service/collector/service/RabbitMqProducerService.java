package simbir.apex.service.collector.service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;
import simbir.apex.service.collector.dtos.ProcessedEventDto;

@Service
public class RabbitMqProducerService {
    private final Logger logger = LoggerFactory.getLogger(this.getClass());
    private final RabbitTemplate rabbitTemplate;
    private final ObjectMapper objectMapper;

    public RabbitMqProducerService(RabbitTemplate rabbitTemplate, ObjectMapper objectMapper) {
        this.rabbitTemplate = rabbitTemplate;
        this.objectMapper = objectMapper;
    }

    private final String exchangeName = "events.exchange";
    private final String routingKey = "events.key";

    public void sendEvent(ProcessedEventDto event) {
        try {
            String messageJson = objectMapper.writeValueAsString(event);
            rabbitTemplate.convertAndSend(exchangeName, routingKey, messageJson);
            logger.info("Sent event to RabbitMQ: {}", messageJson);
        } catch (JsonProcessingException e) {
            logger.error("Failed to serialize event", e);
            throw new RuntimeException("Failed to serialize event", e);
        }
    }
}
