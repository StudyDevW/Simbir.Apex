package simbir.apex.service.collector;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import simbir.apex.service.collector.config.KafkaConfiguration;

@SpringBootApplication
public class CollectorApplication {
    private static final Logger logger = LoggerFactory.getLogger(CollectorApplication.class);

	public static void main(String[] args) {
        logger.info("Starting collector application");
        logger.info("Topic name: {}", KafkaConfiguration.TOPIC);
        SpringApplication.run(CollectorApplication.class, args);
        logger.info("Collector application started");
	}
}
