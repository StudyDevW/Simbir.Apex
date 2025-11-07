package simbir.apex.service.collector.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMqConfiguration {

    private final String exchangeName = "events.exchange";
    private final String queueName = "events.queue";
    private final String routingKey = "events.key";

    @Bean
    public DirectExchange eventsExchange() {
        return new DirectExchange(exchangeName, true, false);
    }

    @Bean
    public Queue eventsQueue() {
        return QueueBuilder.durable(queueName).build();
    }

    @Bean
    public Binding binding(Queue eventsQueue, DirectExchange eventsExchange) {
        return BindingBuilder.bind(eventsQueue).to(eventsExchange).with(routingKey);
    }

    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        return new RabbitTemplate(connectionFactory);
    }
}
