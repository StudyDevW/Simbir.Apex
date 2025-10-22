import { useState } from "react";
import toast from "react-hot-toast";
import useModal from "../../modal/ModalHook";
import RulesApiService from "../service/RulesApiService";

const useRuleDeleteModal = (rulesChangeHandle: () => void) => {
  const { isModalShow, showModal, hideModal } = useModal();
  const [currentId, setCurrentId] = useState<string>("");

  const showModalDialog = (id: string) => {
    showModal();
    setCurrentId(id);
  };

  const onClose = () => {
    hideModal();
  };

  const onDelete = async () => {
    await RulesApiService.delete(currentId);
    rulesChangeHandle();
    toast.success("Правило успешно удалено", { id: "RulesTable" });
    onClose();
  };

  return {
    isDeleteModalShow: isModalShow,
    showDeleteModal: showModalDialog,
    handleDeleteConfirm: onDelete,
    handleDeleteCancel: onClose,
  };
};

export default useRuleDeleteModal;
