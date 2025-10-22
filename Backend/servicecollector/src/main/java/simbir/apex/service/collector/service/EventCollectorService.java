package simbir.apex.service.collector.service;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;
import simbir.apex.service.collector.dtos.EventDto;
import simbir.apex.service.collector.dtos.ProcessedEventDto;
import simbir.apex.service.collector.enums.EventCategory;
import simbir.apex.service.collector.enums.EventSeverity;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.Date;
import java.util.UUID;

@Service
public class EventCollectorService {
    private final Logger logger = LoggerFactory.getLogger(this.getClass());
    private final RabbitMqProducerService rabbitMqProducerService;

    public EventCollectorService(RabbitMqProducerService rabbitMqProducerService) {
        this.rabbitMqProducerService = rabbitMqProducerService;
    }

    public void processEvent(EventDto eventDto) {
        logger.info("Start processing event: {}", eventDto);

        UUID uuid = UUID.randomUUID();
        EventCategory eventCategory = getEventCategory(eventDto.getId());

        if (!isIpValid(eventDto.getIp())) {
            logger.error("Invalid IP address in event: {}", eventDto.getIp());
            logger.info("Event will NOT be sent in RabbitMQ");
            return;
        }
        ProcessedEventDto processed = new ProcessedEventDto(
                uuid,
                eventCategory,
                eventDto.getIp(),
                isIpLAN(eventDto.getIp()),
                eventDto.getDevice(),
                eventDto.getId(),
                getSeverity(eventDto.getId(), eventCategory),
                new Date()
        );

        logger.info("Processed event: {}", processed);
        logger.info("Sending event to RabbitMQ");
        rabbitMqProducerService.sendEvent(processed);

        logger.info("Event sent to RabbitMQ");
    }

    private boolean isIpValid(String ip) {
        InetAddress address;

        try {
            address = InetAddress.getByName(ip);
        } catch (UnknownHostException e) {
            return false;
        }
        if (address.getAddress().length != 4) {
            return false;
        }
        return true;
    }

    private boolean isIpLAN(String ip) {
        InetAddress address;

        try {
            address = InetAddress.getByName(ip);
        } catch (UnknownHostException e) {
            return false;
        }

        byte[] bytes = address.getAddress();
        int firstOctet = bytes[0] & 0xFF;
        int secondOctet = bytes[1] & 0xFF;

        if (firstOctet == 10) {
            return true;
        }
        if (firstOctet == 172 && secondOctet >= 16 && secondOctet <= 31) {
            return true;
        }
        if (firstOctet == 192 && secondOctet == 168) {
            return true;
        }
        if (firstOctet == 169 && secondOctet == 254) {
            return true;
        }
        
        return false;
    }

    private EventSeverity getSeverity(String eventId, EventCategory cat) {
        if (eventId == null) {
            switch (cat) {
                // HIGH
                case FILE_SYSTEM:
                case PROCESS:
                case NETWORK:
                    return EventSeverity.HIGH;

                // MEDIUM
                case HARDWARE:
                    return EventSeverity.MEDIUM;

                // LOW
                case AUTH:
                case UNKNOWN:
                default:
                    return EventSeverity.LOW;
            }
        }
        eventId = eventId.toLowerCase();
        switch (eventId) {
            // HIGH
            case "process_start":
            case "process_stop":
            case "network_connect":
            case "network_disconnect":
            case "usb_connect":
            case "usb_disconnect":
                return EventSeverity.HIGH;

            // MEDIUM
            case "file_edit":
            case "file_save":
            case "file_delete":
            case "login_success":
            case "shutdown":
            case "restart":
            case "config_change":
                return EventSeverity.MEDIUM;

            // LOW
            case "login_fail":
            case "folder_create":
            case "file_open":
            default:
                return EventSeverity.LOW;
        }
    }

    private EventCategory getEventCategory(String eventId) {
        eventId = eventId.toLowerCase();
        if (eventId.startsWith("login") || eventId.startsWith("logout")) {
            return EventCategory.AUTH;
        }
        if (eventId.startsWith("file") || eventId.startsWith("config") || eventId.startsWith("folder")) {
            return EventCategory.FILE_SYSTEM;
        }
        if (eventId.startsWith("process")) {
            return EventCategory.PROCESS;
        }
        if (eventId.startsWith("usb") || eventId.equals("shutdown") || eventId.equals("restart")) {
            return EventCategory.HARDWARE;
        }
        if (eventId.startsWith("network")) {
            return EventCategory.NETWORK;
        }
        return EventCategory.UNKNOWN;
    }

}
