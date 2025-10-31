package simbir.apex.service.agent.service;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.model.RuleDto;

import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Collections;
import java.util.List;

@Service
public class RuleService {
    private final ObjectMapper mapper = new ObjectMapper();

    public List<RuleDto> fetchRules() {
        try {
            URL url = new URL("http://localhost:8080/api/rules"); // TODO: поменять адрес
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestMethod("GET");

            if (conn.getResponseCode() != 200) {
                throw new RuntimeException("Не удалось получить правила: " + conn.getResponseCode());
            }

            return mapper.readValue(conn.getInputStream(), new TypeReference<>() {});
        } catch (Exception e) {
            System.err.println("Ошибка при получении правил: " + e.getMessage());
            return Collections.emptyList();
        }
    }
}
