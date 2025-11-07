package simbir.apex.service.agent.db.jpaRepository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import simbir.apex.service.agent.db.entity.Alert;


import java.util.Date;
import java.util.List;
import java.util.UUID;

@Repository
public interface AlertRepository extends JpaRepository<Alert, UUID> {

    List<Alert> findAllByCreatedAtBetween(Date start, Date end);

}
