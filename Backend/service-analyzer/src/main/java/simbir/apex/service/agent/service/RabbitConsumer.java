package simbir.apex.service.agent.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;
import simbir.apex.service.agent.model.EventDto;
import simbir.apex.service.agent.service.EventProcessor;

@Component
public class RabbitConsumer {

    private final EventProcessor eventProcessor;
    private final ObjectMapper objectMapper = new ObjectMapper();

    public RabbitConsumer(EventProcessor eventProcessor) {
        this.eventProcessor = eventProcessor;
    }

    @RabbitListener(queues = "${app.rabbitmq.queue}")
    public void receiveMessage(String message) {
        try {
            EventDto event = objectMapper.readValue(message, EventDto.class);
            eventProcessor.processEvent(event);
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("Не удалось обработать сообщение из очереди: " + message);
        }
    }
}
