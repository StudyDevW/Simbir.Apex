import { Button } from "react-bootstrap";
import ModalConfirm from "../../modal/ModalConfirm";
import ModalForm from "../../modal/ModalForm";
import useRulesDeleteModal from "../hooks/RulesDeleteModalHook";
import useRulesFormModal from "../hooks/RulesFormModalHook";
import useRules from "../hooks/RulesHook";
import RulesTable from "./RulesTable";
import RulesTableRow from "./RulesTableRow";
import RulesForm from "../form/RulesForm";

const Rules = () => {
  const { rules, handleRulesChange } = useRules();

  const {
    isDeleteModalShow,
    showDeleteModal,
    handleDeleteConfirm,
    handleDeleteCancel,
  } = useRulesDeleteModal(handleRulesChange);

  const {
    isFormModalShow,
    isFormValidated,
    showFormModal,
    currentRule,
    handleRuleChange,
    handleFormSubmit,
    handleFormClose,
  } = useRulesFormModal(handleRulesChange);

  return (
    <>
      <RulesTable>
        {rules.map((rule) => (
          <RulesTableRow
            key={rule.id}
            rule={rule}
            onDelete={() => showDeleteModal(rule.id)}
            onEdit={() => showFormModal(rule.id)}
          />
        ))}
      </RulesTable>
      <div className="d-flex justify-content-center">
        <Button
          variant="primary"
          className="fw-bold px-5 mb-5"
          onClick={() => showFormModal()}
        >
          Добавить правило
        </Button>
      </div>
      <ModalConfirm
        show={isDeleteModalShow}
        onConfirm={handleDeleteConfirm}
        onClose={handleDeleteCancel}
        title="Удаление"
        message="Удалить правило?"
      />
      <ModalForm
        show={isFormModalShow}
        validated={isFormValidated}
        onSubmit={handleFormSubmit}
        onClose={handleFormClose}
        title="Редактирование"
      >
        <RulesForm rule={currentRule} handleChange={handleRuleChange} />
      </ModalForm>
    </>
  );
};

export default Rules;
