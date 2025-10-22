import { useEffect, useState } from "react";
import RulesApiService from "../service/RulesApiService";
import type { RuleFormData } from "../entity/Rule";

const useRuleById = (id?: string) => {
  const emptyRule: RuleFormData = {
    name: "",
    description: "",
    logic: "",
    severity: "",
    status: "",
  };

  const [rule, setRule] = useState<RuleFormData>({ ...emptyRule });

  const getRuleById = async (ruleId?: string) => {
    if (ruleId && ruleId.length > 0) {
      const data = await RulesApiService.get(ruleId);
      setRule(data);
    } else {
      setRule({ ...emptyRule });
    }
  };

  useEffect(() => {
    getRuleById(id);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  return {
    rule,
    setRule,
  };
};

export default useRuleById;
