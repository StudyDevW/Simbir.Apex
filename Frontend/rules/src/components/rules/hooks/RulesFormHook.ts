import { useState } from "react";
import toast from "react-hot-toast";
import useRuleById from "./RuleByidHook";
import RulesApiService from "../service/RulesApiService";
import type { Rule } from "../entity/Rule";

const useRuleForm = (id: string | undefined, rulesChangeHandle?: () => void) => {
  const { rule, setRule } = useRuleById(id);
  const [validated, setValidated] = useState(false);

  const resetValidity = () => {
    setValidated(false);
  };

  const getRuleObject = (formData: Partial<Rule>): Partial<Rule> => {
    return {
      name: formData.name,
      description: formData.description,
      logic: formData.logic,
      severity: formData.severity,
      status: formData.status,
    };
  };

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    const inputName = event.target.name as keyof Rule;
    const inputValue =
      event.target.type === "checkbox"
        ? (event.target as HTMLInputElement).checked
        : event.target.value;
    setRule({
      ...rule,
      [inputName]: inputValue as never,
    });
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    const form = event.currentTarget;
    event.preventDefault();
    event.stopPropagation();
    const body = getRuleObject(rule);
    if (form.checkValidity()) {
      if (id === undefined) {
        await RulesApiService.create(body);
      } else {
        await RulesApiService.update(id, body);
      }
      if (rulesChangeHandle) rulesChangeHandle();
      toast.success("Правило успешно сохранено", { id: "RulesTable" });
      return true;
    }
    setValidated(true);
    return false;
  };

  return {
    rule,
    validated,
    handleSubmit,
    handleChange,
    resetValidity,
    setRule
  };
};

export default useRuleForm;
