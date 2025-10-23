package simbir.apex.service.agent.controller;

import lombok.RequiredArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import simbir.apex.service.agent.db.entity.Alert;
import simbir.apex.service.agent.service.AlertService;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.List;

@RestController
@RequestMapping("/api/alerts")
@RequiredArgsConstructor
public class AlertController {

    private final AlertService alertService;

    /**
     * Получить все алерты
     */
    @GetMapping
    public ResponseEntity<List<Alert>> getAllAlerts() {
        return ResponseEntity.ok(alertService.findAll());
    }

    /**
     * Получить конкретный алерт по ID
     */
    @GetMapping("/{id}")
    public ResponseEntity<Alert> getAlertById(@PathVariable Long id) {
        return ResponseEntity.of(alertService.findById(id));
    }

    /**
     * Подтвердить алерт
     */
    @PostMapping("/{id}/confirm")
    public ResponseEntity<Alert> confirmAlert(@PathVariable Long id) {
        return ResponseEntity.ok(alertService.confirmAlert(id));
    }

    /**
     * Эскалировать алерт
     */
    @PostMapping("/{id}/escalate")
    public ResponseEntity<Alert> escalateAlert(@PathVariable Long id) {
        return ResponseEntity.ok(alertService.escalateAlert(id));
    }

    /**
     * Закрыть алерт с нотами
     */
    @PostMapping("/{id}/resolve")
    public ResponseEntity<Alert> resolveAlert(@PathVariable Long id, @RequestParam(required = false) String notes) {
        return ResponseEntity.ok(alertService.resolveAlert(id, notes));
    }

    /**
     * Пометить как false positive
     */
    @PostMapping("/{id}/false-positive")
    public ResponseEntity<Alert> markFalsePositive(@PathVariable Long id) {
        return ResponseEntity.ok(alertService.markFalsePositive(id));
    }

    @GetMapping("/range")
    public List<Alert> getAlertsByRange(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime start,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime end
    ) {
        return alertService.getByPeriod(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );
    }
}
