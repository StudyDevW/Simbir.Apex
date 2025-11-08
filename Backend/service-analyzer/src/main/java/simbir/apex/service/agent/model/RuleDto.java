package simbir.apex.service.agent.model;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.Date;
import java.util.UUID;

/**
 * DTO для описания правила корреляции событий (шаблона угроз).
 * Пример logic:
 * "action=LOGIN_FAIL;count=10"
 * "type=FILE_ACCESS,action=DELETE;count=3"
 * "action=LOGIN_FAIL;sequence=LOGIN_SUCCESS;count=1"
 */
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class RuleDto {

    private UUID Id;

    private String name;

    private String description;

    private String logic;

    private String severity;

    private String status;

    private UUID created_by;

    private Date created_at;

    private Date updated_at;
}
