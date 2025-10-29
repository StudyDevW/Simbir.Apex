/*
package simbir.apex.service.agent;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.Assertions;
import org.testcontainers.containers.KafkaContainer;
import org.testcontainers.utility.DockerImageName;

import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.time.Duration;
import java.util.*;
import java.util.concurrent.TimeUnit;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.service.KafkaProducerService;
import simbir.apex.service.agent.utils.JsonReader;

public class KafkaTest {
    static KafkaContainer kafka;

    @BeforeAll
    static void setup() {
        kafka = new KafkaContainer(DockerImageName.parse("confluentinc/cp-kafka:7.5.0"));
        kafka.start();
        System.setProperty("KAFKA_BOOTSTRAP_SERVERS", kafka.getBootstrapServers());
    }

    @AfterAll
    static void teardown() {
        kafka.stop();
    }

    @Test
    void testAgentEvents() throws Exception {
        JsonReader.Config config = JsonReader.loadConfig();
        final List<Event> events = config.getEvents();

        KafkaProducerService kafkaProducer = new KafkaProducerService(kafka.getBootstrapServers(), "events-topic");
        for (Event event : events) {
            kafkaProducer.sendEvent(event);
        }
        
        TimeUnit.SECONDS.sleep(1);

        Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, kafka.getBootstrapServers());
        props.put(ConsumerConfig.GROUP_ID_CONFIG, "test-group");
        props.put(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "earliest");
        props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
        props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");

        KafkaConsumer<String, String> consumer = new KafkaConsumer<>(props);
        consumer.subscribe(Collections.singletonList("events-topic"));

        List<String> consumedMessages = new ArrayList<>();
        long timeoutMs = 10_000;
        long start = System.currentTimeMillis();

        while (System.currentTimeMillis() - start < timeoutMs) {
            ConsumerRecords<String, String> records = consumer.poll(Duration.ofSeconds(5));
            for (ConsumerRecord<String, String> record : records) {
                consumedMessages.add(record.value());
            }
            if (consumedMessages.size() >= events.size()) {
                break;
            }
        }

        consumer.close();

        ObjectMapper mapper = new ObjectMapper();
        List<String> sentMessages = events.stream()
                .map(e -> {
                    try {
                        return mapper.writeValueAsString(e);
                    } catch (JsonProcessingException ex) {
                        throw new RuntimeException(ex);
                    }
                })
                .toList();

        Assertions.assertEquals(
                new HashSet<>(sentMessages),
                new HashSet<>(consumedMessages),
                "Полученные события не совпадают с отправленными"
        );
    }
}
*/
