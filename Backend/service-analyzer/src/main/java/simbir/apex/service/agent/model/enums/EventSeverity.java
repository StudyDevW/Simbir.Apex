package simbir.apex.service.agent.model.enums;

import com.fasterxml.jackson.annotation.JsonValue;
import lombok.AllArgsConstructor;

@AllArgsConstructor
public enum EventSeverity {
    LOW("low"),
    MEDIUM("medium"),
    HIGH("high");

    private final String stringValue;

    @JsonValue
    public String getStringValue() {
        return stringValue;
    }
}