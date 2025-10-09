package simbir.apex.service.agent.model;


import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.security.Timestamp;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class EventDto {
    private String id;
    private String device;
    private String ip;
    private Timestamp timestamp;
}