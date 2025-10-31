package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

import java.util.Date;
import java.util.UUID;

@Entity
@Getter
@Setter
@Table(name = "alertsTable")
@Builder
public class Alert {
    @Id
    @GeneratedValue
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

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private String status;

    private String severity;

    @Column(nullable = false, columnDefinition = "TEXT", name = "raw_data ")
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
