package simbir.apex.service.agent;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.AfterAll;
import org.testcontainers.containers.KafkaContainer;
import org.testcontainers.utility.DockerImageName;

import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.time.Duration;
import java.util.*;
import java.util.concurrent.TimeUnit;

import com.fasterxml.jackson.databind.ObjectMapper;

import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.model.Scenario;
import simbir.apex.service.agent.service.EventProvider;
import simbir.apex.service.agent.service.KafkaProducerService;
import simbir.apex.service.agent.service.impl.ScenarioEventProvider;
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
        final List<Scenario> scenarios = config.getScenarios();
        final EventProvider scenarioProvider = new ScenarioEventProvider(events, scenarios);
        List<Event> seriesOfEvents = scenarioProvider.generate();

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

        // Читаем события в течение N секунд
        ConsumerRecords<String, String> records = consumer.poll(Duration.ofSeconds(10));
    }
}
