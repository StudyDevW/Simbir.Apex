//для записи алертов. НАдо будет изменить когда будет бд

package simbir.apex.service.agent.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import simbir.apex.service.agent.model.AlertDto;
import simbir.apex.service.agent.model.enums.Status;

import java.sql.Timestamp;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class AlertService {

    private final AlertRepository alertRepository;

    public void sendAlert(AlertDto alert) {
        alertRepository.save(alert);
    }

    public AlertDto confirmAlert(Long id) {
        return updateStatus(id, Status.CONFIRMED_PRESET, null);
    }

    public AlertDto escalateAlert(Long id) {
        return updateStatus(id, Status.ESCALATED, null);
    }

    public AlertDto resolveAlert(Long id, String notes) {
        return updateStatus(id, Status.RESOLVED, notes);
    }

    public AlertDto markFalsePositive(Long id) {
        return updateStatus(id, Status.FALSE_POSITIVE, null);
    }

    private AlertDto updateStatus(Long id, Status newStatus, String notes) {
        Optional<AlertDto> optional = alertRepository.findById(id);
        if (optional.isEmpty()) {
            throw new IllegalArgumentException("Alert not found with id: " + id);
        }

        AlertDto alert = optional.get();
        alert.setStatus(newStatus);
        if (newStatus == Status.RESOLVED) {
            alert.setClosedAt(new Timestamp(System.currentTimeMillis()));
        }
        if (notes != null) {
            alert.setResolutionNotes(notes);
        }

        alertRepository.save(alert);
        return alert;
    }
}
