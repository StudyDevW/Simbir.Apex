package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.UUID;
import java.util.Date;

@Entity
@Table(name = "rulesTable")
@Getter
@Setter
public class Rule {

    @Id
    @GeneratedValue
    @Column(name="Id")
    private UUID Id;

    @Column(name="name")
    private String name;

    @Column(name="description")
    private String description;

    @Column(name="logic")
    private String logic;

    @Column(name="severity")
    private String severity;

    @Column(name="status")
    private String status;

    @Column(name = "created_by")
    private UUID created_by;

    @Column(nullable = false, name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date created_at;

    @Column(name = "updated_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date updated_at;
}
