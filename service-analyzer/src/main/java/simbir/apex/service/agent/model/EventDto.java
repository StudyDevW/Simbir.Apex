package simbir.apex.service.agent.model;


import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import simbir.apex.service.agent.model.enums.EventCategory;
import simbir.apex.service.agent.model.enums.EventSeverity;

import java.security.Timestamp;
import java.util.Date;
import java.util.UUID;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class EventDto {
    private UUID id;
    private EventCategory category;
    private String ip;
    private boolean isLan;
    private String device;
    private String eventId;
    private EventSeverity severity;
    private Date timestamp;
}