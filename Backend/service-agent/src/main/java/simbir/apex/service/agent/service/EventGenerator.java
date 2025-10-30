package simbir.apex.service.agent.service;

import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.model.Scenario;
import simbir.apex.service.agent.service.impl.ScenarioEventProvider;
import simbir.apex.service.agent.service.impl.SingleEventProvider;
import simbir.apex.service.agent.utils.JsonReader;

import java.io.IOException;
import java.util.List;
import java.util.Random;

public class EventGenerator {

    private final List<Event> events;
    private final List<Scenario> scenarios;

    private final EventProvider singleProvider;
    private final EventProvider scenarioProvider;

    public EventGenerator() throws IOException {
        JsonReader.Config config = JsonReader.loadConfig();
        this.events = config.getEvents();
        this.scenarios = config.getScenarios();

        this.singleProvider = new SingleEventProvider(events);
        this.scenarioProvider = new ScenarioEventProvider(events, scenarios);
    }

    public List<Event> generateRandomEvents() {
        return new Random().nextBoolean()
                ? singleProvider.generate()
                : scenarioProvider.generate();
    }
}
