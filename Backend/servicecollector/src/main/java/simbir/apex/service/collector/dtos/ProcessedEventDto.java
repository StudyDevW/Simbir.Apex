package simbir.apex.service.collector.dtos;


import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import simbir.apex.service.collector.enums.EventCategory;
import simbir.apex.service.collector.enums.EventSeverity;

import java.util.Date;
import java.util.UUID;

@AllArgsConstructor
@Getter
@Setter
public class ProcessedEventDto {
    private UUID id;
    private EventCategory category;
    private String ip;
    private boolean isLan;
    private String device;
    private String eventId;
    private EventSeverity severity;
    private Date timestamp;
}
