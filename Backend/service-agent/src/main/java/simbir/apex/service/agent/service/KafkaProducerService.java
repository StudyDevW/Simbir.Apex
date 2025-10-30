package simbir.apex.service.agent.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.ProducerRecord;
import org.apache.kafka.clients.producer.RecordMetadata;
import simbir.apex.service.agent.model.Event;

import java.util.Properties;
import java.util.concurrent.Future;

public class KafkaProducerService {

    private final KafkaProducer<String, String> producer;
    private final String topic;
    private final ObjectMapper mapper = new ObjectMapper();

    /**
     * Создаёт сервис для отправки событий в Kafka
     *
     * @param bootstrapServers адрес Kafka брокера, например "localhost:9092"
     * @param topic            топик, в который будут отправляться события
     */
    public KafkaProducerService(String bootstrapServers, String topic) {
        this.topic = topic;

        Properties props = new Properties();
        props.put("bootstrap.servers", bootstrapServers);
        props.put("key.serializer", "org.apache.kafka.common.serialization.StringSerializer");
        props.put("value.serializer", "org.apache.kafka.common.serialization.StringSerializer");

        this.producer = new KafkaProducer<>(props);
    }

    /**
     * Отправляет событие в Kafka
     *
     * @param event объект Event
     */
    public void sendEvent(Event event) {
        try {
            // Сериализуем Event в JSON
            String json = mapper.writeValueAsString(event);

            // Создаём запись с ключом event.getId()
            ProducerRecord<String, String> record = new ProducerRecord<>(topic, event.getId(), json);

            // Отправляем в Kafka
            Future<RecordMetadata> future = producer.send(record);
            future.get(); // опционально ждем подтверждения
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * Закрывает Kafka producer
     */
    public void close() {
        producer.close();
    }
}
