import { ApiClient } from 'shell/api';
import { type Rule } from '../entity/Rule'

class RulesApiService {
  private readonly url = 'rules';

  async getAll(): Promise<Rule[]> {
    return ApiClient.get(this.url);
  }

  async get(id: string): Promise<Rule> {
    return ApiClient.get(`${this.url}/${id}`);
  }

  async create(data: Partial<Rule>): Promise<Rule> {
    return ApiClient.post(this.url, data);
  }

  async update(id: string, data: Partial<Rule>): Promise<Rule> {
    return ApiClient.put(`${this.url}/${id}`, data);
  }

  async delete(id: string): Promise<void> {
    return ApiClient.delete(`${this.url}/${id}`);
  }
}

export default new RulesApiService();