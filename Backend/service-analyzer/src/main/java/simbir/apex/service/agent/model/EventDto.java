package simbir.apex.service.agent.model;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import simbir.apex.service.agent.model.enums.EventSeverity;

import java.util.Date;
import java.util.UUID;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
@JsonIgnoreProperties(ignoreUnknown = true)
public class EventDto {
    @JsonProperty("Id") 
    private UUID id;
    private boolean isLan;
    @JsonProperty("Device") 
    private String device;
    @JsonProperty("EventId") 
    private String eventId;
    @JsonProperty("Severity") 
    private EventSeverity severity;
    @JsonProperty("Timestamp") 
    private Date timestamp;
}