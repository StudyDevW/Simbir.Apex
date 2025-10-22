import { useEffect, useState } from "react";
import RulesApiService from "../service/RulesApiService";
import type { Rule } from "../entity/Rule";

const useRules = () => {
  const [rules, setRules] = useState<Rule[]>([]);
  const [rulesRefresh, setRulesRefresh] = useState(false);

  const handleRulesChange = () => setRulesRefresh((prev) => !prev);

  const getRules = async () => {
    try {
      const data = await RulesApiService.getAll();
      setRules(data ?? []);
    } catch (e) {
      console.error("Failed to fetch rules:", e);
      setRules([]);
    }
  };

  useEffect(() => {
    getRules();
  }, [rulesRefresh]);

  return {
    rules,
    handleRulesChange,
  };
};

export default useRules;
