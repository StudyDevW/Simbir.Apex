package simbir.apex.service.agent.сonsumer;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.listener.SimpleMessageListenerContainer;
import org.springframework.amqp.rabbit.listener.adapter.MessageListenerAdapter;
import org.springframework.stereotype.Component;

@Component
public class DynamicRabbitConsumer {

    private final ConnectionFactory connectionFactory;
    private final AmqpAdmin amqpAdmin;

    public DynamicRabbitConsumer(ConnectionFactory connectionFactory, AmqpAdmin amqpAdmin) {
        this.connectionFactory = connectionFactory;
        this.amqpAdmin = amqpAdmin;
    }

    public void startConsumer(String queueName, String exchangeName, String routingKey) {
        // Создаём exchange и очередь динамически
        DirectExchange exchange = new DirectExchange(exchangeName);
        Queue queue = new Queue(queueName, true);
        Binding binding = BindingBuilder.bind(queue).to(exchange).with(routingKey);

        amqpAdmin.declareExchange(exchange);
        amqpAdmin.declareQueue(queue);
        amqpAdmin.declareBinding(binding);

        // Настраиваем listener
        SimpleMessageListenerContainer container = new SimpleMessageListenerContainer();
        container.setConnectionFactory(connectionFactory);
        container.setQueues(queue);
        container.setMessageListener(new MessageListenerAdapter((MessageListener) message -> {
            String body = new String(message.getBody());
            System.out.println("Received: " + body);
            // TODO: обработка данных
        }));

        container.start();
        System.out.println("Consumer started for queue: " + queueName);
    }
}
