package simbir.apex.service.agent.db.jpaRepository;


import org.springframework.data.jpa.repository.JpaRepository;
import simbir.apex.service.agent.db.entity.Alert;

import java.sql.Timestamp;
import java.util.List;

public interface AlertRepository extends JpaRepository<Alert, Long> {

    List<Alert> findAllByCreatedAtBetween(Timestamp start, Timestamp end);
}
