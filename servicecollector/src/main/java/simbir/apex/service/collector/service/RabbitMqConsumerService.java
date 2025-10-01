/*Написано для Теста RabbitMQ */

package simbir.apex.service.collector.service;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Service;

@Service
public class RabbitMqConsumerService {

    @RabbitListener(queues = "events.queue")
    public void receiveMessage(String message) {
        System.out.println(" [x] Received from RabbitMQ: " + message);
    }
}
