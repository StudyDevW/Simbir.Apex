package simbir.apex.service.agent;


import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.service.EventGenerator;
import simbir.apex.service.agent.service.KafkaProducerService;

import java.io.IOException;
import java.util.List;
import java.util.concurrent.TimeUnit;

public class AgentSimulatorApp {

    public static void main(String[] args) throws IOException, InterruptedException {

        EventGenerator generator = new EventGenerator();

        KafkaProducerService kafkaService = new KafkaProducerService("localhost:9092", "events-topic");

        System.out.println("Agent Simulator started. Sending events to Kafka...");

        while (true) {
            List<Event> events = generator.generateRandomEvents();

            for (Event event : events) {
                kafkaService.sendEvent(event);
                System.out.println("Sent event: " + event);
            }

            // Пауза между пакетами событий (2 секунды)
            TimeUnit.SECONDS.sleep(2);
        }


    }
}
