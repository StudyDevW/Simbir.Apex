package simbir.apex.service.agent.db.jpaRepository;


import org.springframework.data.jpa.repository.JpaRepository;
import simbir.apex.service.agent.db.entity.Alert;

public interface AlertRepository extends JpaRepository<Alert, Long> {
}
