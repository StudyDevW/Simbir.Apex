package simbir.apex.service.agent.utils;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import simbir.apex.service.agent.model.Event;
import simbir.apex.service.agent.model.Scenario;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;

public class JsonReader {

    private static final ObjectMapper mapper = new ObjectMapper();

    private JsonReader() { }

    public static Config loadConfig() throws IOException {
        List<Event> events = new ArrayList<>();
        List<Scenario> scenarios = new ArrayList<>();

        try (InputStream is = JsonReader.class.getClassLoader().getResourceAsStream("events.json")) {
            if (is == null) throw new IOException("events.json not found");

            JsonNode root = mapper.readTree(is);

            for (JsonNode e : root.get("events")) {
                events.add(new Event(
                        e.get("id").asText(),
                        e.get("device").asText(),
                        e.get("ip").asText(),
                        System.currentTimeMillis()
                ));
            }

            for (JsonNode s : root.get("scenarios")) {
                List<String> seq = new ArrayList<>();
                for (JsonNode ev : s.get("sequence")) {
                    seq.add(ev.asText());
                }
                scenarios.add(new Scenario(s.get("name").asText(), seq));
            }
        }

        return new Config(events, scenarios);
    }

    public static class Config {
        private final List<Event> events;
        private final List<Scenario> scenarios;

        public Config(List<Event> events, List<Scenario> scenarios) {
            this.events = events;
            this.scenarios = scenarios;
        }

        public List<Event> getEvents() { return events; }
        public List<Scenario> getScenarios() { return scenarios; }
    }
}
