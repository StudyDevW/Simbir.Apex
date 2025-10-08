package simbir.apex.service.agent.service;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import simbir.apex.service.agent.сonsumer.DynamicRabbitConsumer;


@Service
public class RabbitInitializerService {

    private final DynamicRabbitConsumer dynamicConsumer;
    private final RestTemplate restTemplate = new RestTemplate();
    private final ObjectMapper objectMapper = new ObjectMapper();

    public RabbitInitializerService(DynamicRabbitConsumer dynamicConsumer) {
        this.dynamicConsumer = dynamicConsumer;
    }

    public void initConsumer() {
        try {
            // Асинхронный запрос к сервису управленцу
            String url = "";
            String response = restTemplate.getForObject(url, String.class);

            // Разбираем JSON
            JsonNode json = objectMapper.readTree(response);
            String queue = json.get("queue").asText();
            String exchange = json.get("exchange").asText();
            String routingKey = json.get("routingKey").asText();

            // Запускаем динамический consumer
            dynamicConsumer.startConsumer(queue, exchange, routingKey);

        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("Не удалось инициализировать RabbitMQ consumer, попробуйте позже");
        }
    }
}
