import { useEffect, useState } from "react";
import RulesApiService from "../service/RulesApiService";
import type { Rule } from "../entity/Rule";

const useRules = (page = 1) => {
  const [rules, setRules] = useState<Rule[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [rulesRefresh, setRulesRefresh] = useState(false);

  const handleRulesChange = () => setRulesRefresh((prev) => !prev);

  const getRules = async () => {
    try {
      const response = await RulesApiService.getAll({ page });

      setRules(response.items ?? []);
      setTotalPages(response.totalPages ?? 1);
    } catch (e) {
      console.error("Failed to fetch rules:", e);
      setRules([]);
      setTotalPages(1);
    }
  };

  useEffect(() => {
    getRules();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, rulesRefresh]);

  return {
    rules,
    totalPages,
    handleRulesChange,
  };
};

export default useRules;
