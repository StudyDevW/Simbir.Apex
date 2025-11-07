import { useState } from "react";

const useModal = () => {
  const [showModal, setShowModal] = useState<boolean>(false);

  const showModalDialog = () => {
    setShowModal(true);
  };

  const hideModalDialog = () => {
    setShowModal(false);
  };

  return {
    isModalShow: showModal,
    showModal: showModalDialog,
    hideModal: hideModalDialog,
  };
};

export default useModal;
