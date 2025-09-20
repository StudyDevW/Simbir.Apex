package simbir.apex.service.agent.model;

import lombok.AllArgsConstructor;
import lombok.Data;

import java.util.List;

@Data
@AllArgsConstructor
public class Scenario {
    private String name;
    private List<String> sequence;
}