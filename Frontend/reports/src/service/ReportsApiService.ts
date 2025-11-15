import { ApiClient } from 'shell/api';
import type { Reports } from '../assets/reports/Reports';

// interface PaginatedResponse<T> {
//   items: T[];
//   totalPages: number;
// }

class ReportsApiService {
    private readonly url = 'Reports';

    async getAll(params?: { page?: number }): Promise<Reports[]> {
        return ApiClient.get(`${this.url}/All`, { params });
    }

    async get(id: string): Promise<Reports> {
        return ApiClient.get(`${this.url}/${id}`);
    }

    async create(data: Partial<Reports>): Promise<Reports> {
        return ApiClient.post(`${this.url}/Add`, data);
    }

    async update(id: string, data: Partial<Reports>): Promise<Reports> {
        return ApiClient.put(`${this.url}/Change/${id}`, data);
    }

    async delete(id: string): Promise<void> {
        return ApiClient.delete(`${this.url}/Delete/${id}`);
    }
}

export default new ReportsApiService();