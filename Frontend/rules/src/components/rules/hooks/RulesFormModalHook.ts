import { useState } from "react";
import useModal from "../../modal/ModalHook";
import useRuleForm from "./RulesFormHook";

const useRuleFormModal = (rulesChangeHandle?: () => void) => {
  const { isModalShow, showModal, hideModal } = useModal();
  const [currentId, setCurrentId] = useState<string | undefined>(undefined);

  const {
    rule,
    validated,
    handleSubmit,
    handleChange,
    resetValidity,
    setRule,
  } = useRuleForm(currentId, rulesChangeHandle);

  const showModalDialog = (id?: string) => {
    setCurrentId(id);
    resetValidity();
    showModal();
  };

  const onClose = () => {
    setCurrentId(undefined);
    setRule({
        name: "",
        description: "",
        logic: "",
        severity: "",
        status: "",
      })
    hideModal();
  };

  const onSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    if (await handleSubmit(event)) {
      onClose();
    }
  };

  return {
    isFormModalShow: isModalShow,
    isFormValidated: validated,
    showFormModal: showModalDialog,
    currentRule: rule,
    handleRuleChange: handleChange,
    handleFormSubmit: onSubmit,
    handleFormClose: onClose,
  };
};

export default useRuleFormModal;
