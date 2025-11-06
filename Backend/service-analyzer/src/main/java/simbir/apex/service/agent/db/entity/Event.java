package simbir.apex.service.agent.db.entity;

import jakarta.persistence.*;
import lombok.*;
import simbir.apex.service.agent.model.enums.EventSeverity;

import java.util.Date;
import java.util.UUID;

@Entity
@Table(name = "\"eventsTable\"")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Event {

    @Id
    @GeneratedValue
    private UUID id;

    @Column(name="is_Lan")
    private boolean isLan;

    private String device;

    @Column(name="event_Id")
    private String eventId;

    @Enumerated(EnumType.STRING)
    private EventSeverity severity;

    @Temporal(TemporalType.TIMESTAMP)
    private Date timestamp;
}
