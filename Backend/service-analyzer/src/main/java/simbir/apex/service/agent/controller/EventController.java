package simbir.apex.service.agent.controller;

import lombok.RequiredArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.web.bind.annotation.*;
import simbir.apex.service.agent.db.entity.Event;
import simbir.apex.service.agent.service.EventService;

import java.util.Date;
import java.util.List;

@RestController
@RequestMapping("/api/events")
@RequiredArgsConstructor
public class EventController {

    private final EventService eventService;

    @GetMapping("/period")
    public List<Event> getEventsByPeriod(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) Date start,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) Date end
    ) {
        return eventService.getEventsBetween(start, end);
    }
}
