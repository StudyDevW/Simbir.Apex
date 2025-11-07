package simbir.apex.service.agent.config;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.HttpEntity;
import org.springframework.http.MediaType;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;
import simbir.apex.service.agent.model.AppInfoDto;
import simbir.apex.service.agent.model.IPEndPoint;


import java.net.InetAddress;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.UnknownHostException;
import java.util.HashMap;
import java.util.Map;

@Configuration
public class ManagerRegistrationConfiguration {
    private final Logger logger = LoggerFactory.getLogger(ManagerRegistrationConfiguration.class);

    @Autowired
    private ObjectMapper objectMapper;

    @Autowired
    Environment env;

    @Bean
    boolean registerInManager() {
        logger.info("Trying to register in manager");
        String managerUrl = env.getProperty("app.manager.url");
        String managerPort = env.getProperty("app.manager.port");

        String appPort = env.getProperty("server.port");
        String managerUrlWithPort = String.format("%s:%s", managerUrl, managerPort);

        if (managerUrl == null || managerPort == null) {
            logger.error("Manager URL or Port is not specified");
            return false;
        }

        if (appPort == null) {
            logger.error("App port is not specified somehow");
            return false;
        }

        logger.info("Manager URL: {}", managerUrlWithPort);

        try {
            String ip = InetAddress.getLocalHost().getHostAddress();
            int integerPort = Integer.parseInt(appPort);

            URI managerUri = new URI("http://" + managerUrlWithPort + "/api/Main/InsertService");

            AppInfoDto serviceRecord = new AppInfoDto(
                    "ServiceAgent",  // ServiceName
                    new IPEndPoint(ip, integerPort),  // EndPointService
                    0
            );

            objectMapper.setPropertyNamingStrategy(PropertyNamingStrategies.UPPER_CAMEL_CASE);

            String json = objectMapper.writeValueAsString(serviceRecord);

            String postJson = objectMapper.writeValueAsString(json);

            RestTemplate restTemplate = new RestTemplate();

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_JSON);

            logger.info("Строка после двойной сериализации: {}", postJson);

            HttpEntity<String> request = new HttpEntity<>(postJson, headers);

            ResponseEntity<String> response = restTemplate.exchange(
                    managerUri,
                    HttpMethod.POST,
                    request,
                    String.class
            );

            if (response.getStatusCode() == HttpStatus.OK) {
                logger.info("Successfully registered in manager");
                return true;
            }

            logger.error("Failed to register in manager. Message from Manager: {}", response.getBody());
            return false;
        } catch (URISyntaxException e) {
            logger.error("Error while parsing URI: {}", managerUrl, e);
            return false;
        } catch (UnknownHostException e) {
            logger.error("Error while getting IP:", e);
            return false;
        } catch (Exception e) {
            logger.error("Error while registering in manager", e);
            return false;
        }
    }
}