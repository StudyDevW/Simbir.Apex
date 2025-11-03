package simbir.apex.service.agent.service;

import org.springframework.stereotype.Service;
import simbir.apex.service.agent.db.jpaRepository.RuleRepository;
import simbir.apex.service.agent.model.RuleDto;


import java.util.List;

@Service
public class RuleService {

    private final RuleRepository ruleRepository;

    public RuleService(RuleRepository ruleRepository) {
        this.ruleRepository = ruleRepository;
    }

    public List<RuleDto> fetchRules() {
        try {
            return ruleRepository.findAllAsDto();
        } catch (Exception e) {
            System.err.println("Ошибка при получении правил из БД: " + e.getMessage());
            return List.of();
        }
    }
}
