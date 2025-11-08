package simbir.apex.service.agent.db.entity;

import simbir.apex.service.agent.model.enums.EventSeverity;

import jakarta.persistence.*;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.*;
import java.util.Date;
import java.util.UUID;

@Entity
@Table(name = "alertsTable")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Alert {

    @Id
    @GeneratedValue
    @Column(name = "Id")
    private UUID id;

    @Column(name = "rule_id")
    private UUID ruleId;

    @Column(name = "assigned_to")
    private UUID assignedTo;

    @Column(nullable = false)
    private String hostname;

    @Column(nullable = false)
    private String title;

    @Column(nullable = false, columnDefinition = "TEXT")
    private String description;

    @Column(nullable = false, columnDefinition = "TEXT")
    private String status;

    @Enumerated(EnumType.STRING)
    private EventSeverity severity;

    @Column(nullable = false, columnDefinition = "TEXT", name = "raw_data")
    private String rawData;

    @Column(nullable = false, name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date createdAt;

    @Column(name = "closed_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date closedAt;

    @Column(name = "resolution_notes")
    private String resolutionNotes;
}
