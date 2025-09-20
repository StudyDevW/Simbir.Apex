package simbir.apex.service.agent.service.impl;

import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.model.Scenario;
import simbir.apex.service.agent.service.EventProvider;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class ScenarioEventProvider implements EventProvider {
    private final List<Event> events;
    private final List<Scenario> scenarios;
    private final Random random = new Random();

    public ScenarioEventProvider(List<Event> events, List<Scenario> scenarios) {
        this.events = events;
        this.scenarios = scenarios;
    }

    @Override
    public List<Event> generate() {
        Scenario s = scenarios.get(random.nextInt(scenarios.size()));
        List<Event> result = new ArrayList<>();
        for (String id : s.getSequence()) {
            events.stream()
                    .filter(ev -> ev.getId().equals(id))
                    .findFirst()
                    .ifPresent(ev -> result.add(
                            new Event(ev.getId(), ev.getType(), System.currentTimeMillis())
                    ));
        }
        return result;
    }
}
