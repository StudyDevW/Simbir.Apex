package simbir.apex.service.agent;

import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import simbir.apex.service.agent.service.RabbitInitializerService;


@SpringBootApplication
public class AnalyzerApplication implements CommandLineRunner {

    private final RabbitInitializerService rabbitInitializerService;

    public AnalyzerApplication(RabbitInitializerService rabbitInitializerService) {
        this.rabbitInitializerService = rabbitInitializerService;
    }

    public static void main(String[] args) {
        SpringApplication.run(AnalyzerApplication.class, args);
    }

    @Override
    public void run(String... args) {
        // Асинхронно запускаем consumer после получения конфигурации
        new Thread(rabbitInitializerService::initConsumer).start();
    }
}
