package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.*;
import simbir.apex.service.agent.model.enums.Status;

import java.sql.Timestamp;

@Entity
@Table(name = "alerts")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Alert {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private Long ruleId; // nullable
    private String assignedTo; // nullable
    private String hostname;
    private String title;
    private String description;

    @Enumerated(EnumType.STRING)
    private Status status;

    private String severity; // nullable

    @Column(columnDefinition = "TEXT")
    private String rawData; // JSON сериализация списка событий

    @Builder.Default
    private Timestamp createdAt = new Timestamp(System.currentTimeMillis());

    private Timestamp closedAt; // nullable
    private String resolutionNotes; // nullable
}
