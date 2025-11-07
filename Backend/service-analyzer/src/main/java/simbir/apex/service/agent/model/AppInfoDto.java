package simbir.apex.service.agent.model;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class AppInfoDto {
    private String ServiceName;

    private IPEndPoint EndPointService;

    private int Id;
}