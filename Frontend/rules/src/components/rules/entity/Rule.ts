export interface Rule {
  id: string;
  name: string;
  description: string;
  logic: string;
  severity: string;
  status: string;
  created_by: string;
  created_at: string;
  updated_at: string;
}

export type RuleFormData = Omit<Rule, "id" | "created_by" | "created_at" | "updated_at">;