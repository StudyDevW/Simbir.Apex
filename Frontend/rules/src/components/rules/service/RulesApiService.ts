import { ApiClient } from 'shell/api';
import type { Rule } from '../entity/Rule';

interface PaginatedResponse<T> {
  items: T[];
  totalPages: number;
}

class RulesApiService {
  private readonly url = 'Rules';

  async getAll(params?: { page?: number }): Promise<PaginatedResponse<Rule>> {
    return ApiClient.get(`${this.url}/All`, { params });
  }

  async get(id: string): Promise<Rule> {
    return ApiClient.get(`${this.url}/${id}`);
  }

  async create(data: Partial<Rule>): Promise<Rule> {
    return ApiClient.post(`${this.url}/TimedRules/Add`, data);
  }

  async update(id: string, data: Partial<Rule>): Promise<Rule> {
    return ApiClient.put(`${this.url}/${id}`, data);
  }

  async delete(id: string): Promise<void> {
    return ApiClient.delete(`${this.url}/${id}`);
  }
}

export default new RulesApiService();