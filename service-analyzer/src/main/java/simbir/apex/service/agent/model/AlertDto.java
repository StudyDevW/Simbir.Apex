package simbir.apex.service.agent.model;

import java.sql.Timestamp;
import java.util.List;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import simbir.apex.service.agent.model.EventDto;
import simbir.apex.service.agent.model.enums.Status;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class AlertDto {
    private Long id;
    private Long ruleId;
    private Long assignedTo;
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
