package simbir.apex.service.collector.config;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;
import simbir.apex.service.collector.dtos.AppInfoDto;
import simbir.apex.service.collector.dtos.EndpointService;

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

            URI managerUri = new URI("http://" + managerUrlWithPort + "/api/main/insertservice");
            String urlTemplate = UriComponentsBuilder.fromUri(managerUri)
                    .queryParam("jsonData", "{jsonData}")
                    .encode()
                    .toUriString();

            Map<String, String> params = new HashMap<>();

            params.put("jsonData",objectMapper.writeValueAsString(new AppInfoDto(
                    "collector",
                    new EndpointService(ip, integerPort)
            )));

            RestTemplate temp = new RestTemplate();

            ResponseEntity<?> entity = temp.exchange(
                    urlTemplate,
                    HttpMethod.POST,
                    null,
                    String.class, params
            );
            if (entity.getStatusCode() == HttpStatus.OK) {
                logger.info("Successfully registered in manager");
                return true;
            }

            logger.error("Failed to register in manager. Message from Manager: {}", entity.getBody());
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
