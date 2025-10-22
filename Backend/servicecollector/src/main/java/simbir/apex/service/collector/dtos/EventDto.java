package simbir.apex.service.collector.dtos;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.Date;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class EventDto {
    private String id;
    private String device;
    private String ip;
    private Date timestamp;
}
