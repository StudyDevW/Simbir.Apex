package simbir.apex.service.agent.model;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import simbir.apex.service.agent.model.enums.Status;

import java.sql.Timestamp;
import java.util.List;
import java.util.UUID;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class AlertDto {
    private UUID id;
    private UUID ruleId;
    private UUID assignedTo;
    private String hostname;
    private String title;
    private String description;
    private Status status;

    private String severity;
    private List<EventDto> rawData;
    private Timestamp createdAt;
    private Timestamp closedAt;
    private String resolutionNotes;
}
