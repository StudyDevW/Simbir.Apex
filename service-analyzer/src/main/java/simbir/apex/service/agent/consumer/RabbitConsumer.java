package simbir.apex.service.agent.consumer;

import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;
import simbir.apex.service.agent.model.EventDto;
import simbir.apex.service.agent.service.EventProcessor;

@Component
@RequiredArgsConstructor
public class RabbitConsumer {

    private final EventProcessor eventProcessor;
    private final ObjectMapper objectMapper = new ObjectMapper();

    /**
     * Слушает очередь, имя которой берется из application.yml
     */
    @RabbitListener(queues = "${app.rabbitmq.queue}")
    public void receiveMessage(String message) {
        try {
            // Преобразуем JSON из очереди в EventDto
            EventDto event = objectMapper.readValue(message, EventDto.class);

            // Передаём событие в EventProcessor
            eventProcessor.processEvent(event);

        } catch (Exception e) {
            System.err.println("Ошибка при обработке сообщения из RabbitMQ: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
