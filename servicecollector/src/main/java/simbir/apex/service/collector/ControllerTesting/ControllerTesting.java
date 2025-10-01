package simbir.apex.service.collector.ControllerTesting;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import simbir.apex.service.collector.handler.EventDto;
import simbir.apex.service.collector.service.RabbitMqProducerService;


import java.time.Instant;
import java.util.Date;

@RestController
public class ControllerTesting {
    private final RabbitMqProducerService producer;
    public ControllerTesting(RabbitMqProducerService producer) {
        this.producer = producer;
    }

    @PostMapping("/send")
    public String sendEvent(@RequestParam String eventId) {
        EventDto event = new EventDto(
                eventId,
                "desktop",
                "192.168.0.10",
                Date.from(Instant.now())
        );
        producer.sendEvent(event);
        return "Event sent: " + eventId;
    }
}
