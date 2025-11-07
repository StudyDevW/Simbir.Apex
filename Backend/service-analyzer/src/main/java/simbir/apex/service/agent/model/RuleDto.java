package simbir.apex.service.agent.model;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

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

    private UUID id;

    /**
     * Условие проверки событий.
     * Может содержать:
     * - простое условие: action=LOGIN_FAIL
     * - несколько условий через запятую: type=FILE_ACCESS,action=DELETE
     * - счётчик повторов: count=10
     * - возможные последовательности: sequence=LOGIN_SUCCESS
     */
    private String logic;

    /**
     * Имя правила (для читаемости в UI или в логах).
     */
    private String name;

    /**
     * Краткое описание (опционально, может быть null).
     */
    private String description;
}
