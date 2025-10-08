package simbir.apex.service.collector.enums;

import com.fasterxml.jackson.annotation.JsonValue;
import lombok.AllArgsConstructor;

@AllArgsConstructor
public enum EventCategory {
    UNKNOWN("unknown"),
    AUTH("auth"),
    FILE_SYSTEM("file_system"),
    PROCESS("process"),
    HARDWARE("hardware"),
    NETWORK("network");

    private final String stringValue;

    @JsonValue
    public String getStringValue() {
        return stringValue;
    }

}
