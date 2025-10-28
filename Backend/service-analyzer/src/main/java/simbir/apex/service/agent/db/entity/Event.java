package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.*;
import simbir.apex.service.agent.model.enums.EventSeverity;

import java.util.Date;

@Entity
@Table(name = "events")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Event {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private boolean isLan;

    private String device;

    private String eventId;

    @Enumerated(EnumType.STRING)
    private EventSeverity severity;

    @Temporal(TemporalType.TIMESTAMP)
    private Date timestamp;
}
