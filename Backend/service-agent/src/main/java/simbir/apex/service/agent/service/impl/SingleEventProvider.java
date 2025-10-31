package simbir.apex.service.agent.service.impl;

import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.service.EventProvider;

import java.util.List;
import java.util.Random;

public class SingleEventProvider implements EventProvider {
    private final List<Event> events;
    private final Random random = new Random();

    public SingleEventProvider(List<Event> events) {
        this.events = events;
    }

    @Override
    public List<Event> generate() {
        Event base = events.get(random.nextInt(events.size()));
        return List.of(new Event(base.getId(), base.getDevice(), base.getIp(), System.currentTimeMillis()));
    }
}
