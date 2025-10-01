package simbir.apex.service.collector.handler;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.Date;

@NoArgsConstructor
@Getter
@Setter
public class EventDto {
    private String id;
    private String device;
    private String ip;
    private Date timestamp;
    public EventDto(String id, String device, String ip, Date timestamp) {
        this.id = id;
        this.device = device;
        this.ip = ip;
        this.timestamp = timestamp;
    }
}
