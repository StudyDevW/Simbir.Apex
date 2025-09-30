package simbir.apex.service.collector.service;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;
import simbir.apex.service.collector.handler.EventDto;

@Service
public class EventCollectorService {
    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public void processEvent(EventDto eventDto) {
        logger.info("Start processing event: {}", eventDto);
        //TODO: implement processing
    }

}
