package simbir.apex.service.agent.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.db.entity.Event;
import simbir.apex.service.agent.db.jpaRepository.EventRepository;

import java.util.Date;
import java.util.List;

@Service
@RequiredArgsConstructor
public class EventService {

    private final EventRepository eventRepository;

    public Event saveEvent(Event event) {
        return eventRepository.save(event);
    }

    public List<Event> getEventsBetween(Date start, Date end) {
        return eventRepository.findAllByTimestampBetween(start, end);
    }
}
