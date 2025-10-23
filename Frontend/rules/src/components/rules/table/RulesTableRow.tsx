import { PencilFill, Trash3 } from "react-bootstrap-icons";
import type { Rule } from "../entity/Rule";

interface RulesTableRowProps {
  rule: Rule;
  onDelete: () => void;
  onEdit: () => void;
}

const RulesTableRow = ({ rule, onDelete, onEdit }: RulesTableRowProps) => {
  const handleAnchorClick = (
    event: React.MouseEvent<HTMLAnchorElement>,
    action: () => void
  ) => {
    event.preventDefault();
    action();
  };

  return (
    <tr>
      <td>{rule.name}</td>
      <td>{rule.description}</td>
      <td>{rule.severity}</td>
      <td>{rule.status}</td>
      <td>{rule.created_by}</td>
      <td>{rule.created_at}</td>
      <td>{rule.updated_at}</td>
      <td>
        <a href="#" onClick={(event) => handleAnchorClick(event, onEdit)}>
          <PencilFill />
        </a>
      </td>
      <td>
        <a href="#" onClick={(event) => handleAnchorClick(event, onDelete)}>
          <Trash3 />
        </a>
      </td>
    </tr>
  );
};

export default RulesTableRow;
