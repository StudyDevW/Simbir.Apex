package simbir.apex.service.agent.db.jpaRepository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import simbir.apex.service.agent.db.entity.Rule;
import simbir.apex.service.agent.model.RuleDto;


import java.util.List;
import java.util.UUID;

@Repository
public interface RuleRepository extends JpaRepository<Rule, UUID> {

    @Query("SELECT new simbir.apex.service.agent.dto.RuleDto(r.name, r.description, r.logic, r.severity, r.status) FROM Rule r")
    List<RuleDto> findAllAsDto();
}
