package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "rulesTable")
@Getter
@Setter
public class Rule {

    @Id
    @GeneratedValue
    private UUID id;

    private String name;
    private String description;
    private String logic;
    private String severity;
    private String status;

    @Column(name = "created_by")
    private UUID createdBy;

    @Column(name = "created_at")
    private LocalDateTime createdAt;

    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
}
