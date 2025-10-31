package simbir.apex.service.agent.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.db.entity.Alert;
import simbir.apex.service.agent.db.jpaRepository.AlertRepository;
import simbir.apex.service.agent.model.enums.Status;

import java.sql.Timestamp;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class AlertService {

    private AlertRepository alertRepository;

    public Alert sendAlert(Alert alert) {
        return alertRepository.save(alert);
    }

    public List<Alert> findAll() {
        return alertRepository.findAll();
    }

    public Optional<Alert> findById(UUID id) {
        return alertRepository.findById(id);
    }

    public Alert confirmAlert(UUID id) {
        return updateStatus(id, Status.CONFIRMED_PRESET, null);
    }

    public Alert escalateAlert(UUID id) {
        return updateStatus(id, Status.ESCALATED, null);
    }

    public Alert resolveAlert(UUID id, String notes) {
        return updateStatus(id, Status.RESOLVED, notes);
    }

    public Alert markFalsePositive(UUID id) {
        return updateStatus(id, Status.FALSE_POSITIVE, null);
    }

    public List<Alert> getByPeriod(Timestamp start, Timestamp end) {
        return alertRepository.findAllByCreatedAtBetween(start, end);
    }

    private Alert updateStatus(UUID id, Status newStatus, String notes) {
        Optional<Alert> optional = alertRepository.findById(id);
        if (optional.isEmpty()) {
            throw new IllegalArgumentException("Alert not found with id: " + id);
        }

        Alert alert = optional.get();
        alert.setStatus(String.valueOf(newStatus));
        if (newStatus == Status.RESOLVED) {
            alert.setClosedAt(new Timestamp(System.currentTimeMillis()));
        }
        if (notes != null) {
            alert.setResolutionNotes(notes);
        }

        return alertRepository.save(alert);
    }
}
