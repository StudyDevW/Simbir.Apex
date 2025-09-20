package simbir.apex.service.agent.service;

import simbir.apex.service.agent.model.Event;

import java.util.List;

public interface EventProvider {
    List<Event> generate();
}
