import { Form } from "react-bootstrap";
import type { RuleFormData } from "../entity/Rule";

interface RulesFormProps {
  rule: RuleFormData;
  handleChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
}

const RulesForm = ({ rule, handleChange }: RulesFormProps) => {
  return (
    <>
      <Form.Group className="mb-3" controlId="ruleName">
        <Form.Label>Название</Form.Label>
        <Form.Control
          type="text"
          name="name"
          value={rule.name ?? ""}
          onChange={handleChange}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="ruleDescription">
        <Form.Label>Описание</Form.Label>
        <Form.Control
          as="textarea"
          rows={3}
          name="description"
          value={rule.description ?? ""}
          onChange={handleChange}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="ruleLogic">
        <Form.Label>Логика</Form.Label>
        <Form.Control
          as="textarea"
          rows={3}
          name="logic"
          value={rule.logic ?? ""}
          onChange={handleChange}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="ruleSeverity">
        <Form.Label>Severity</Form.Label>
        <Form.Select
          name="severity"
          value={rule.severity ?? ""}
          onChange={handleChange}
          required
        >
          <option value="">Выберите...</option>
          <option value="low">Низкий</option>
          <option value="medium">Средний</option>
          <option value="high">Высокий</option>
          <option value="critical">Критический</option>
        </Form.Select>
      </Form.Group>

      <Form.Group className="mb-3" controlId="ruleStatus">
        <Form.Label>Статус</Form.Label>
        <Form.Select
          name="status"
          value={rule.status ?? ""}
          onChange={handleChange}
          required
        >
          <option value="">Выберите...</option>
          <option value="active">Активный</option>
          <option value="inactive">Неактивный</option>
        </Form.Select>
      </Form.Group>
    </>
  );
};

export default RulesForm;
