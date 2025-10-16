package simbir.apex.service.agent.db.jpaRepository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import simbir.apex.service.agent.db.entity.Alert;

@Repository
public interface AlertRepository extends JpaRepository<Alert, Long> {
}
