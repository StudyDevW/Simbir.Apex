package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.*;
import simbir.apex.service.agent.model.enums.EventSeverity;

import java.util.Date;
import java.util.UUID;

@Entity
@Table(name = "eventsTable")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Event {

    @Id
    @GeneratedValue
    @Column(name="Id")
    private UUID id;

    @Column(name="isLan")
    private boolean isLan;

    @Column(name="device")
    private String device;

    @Column(name="eventId")
    private String eventId;

    @Enumerated(EnumType.STRING)
    private EventSeverity severity;

    @Temporal(TemporalType.TIMESTAMP)
    private Date timestamp;
}
