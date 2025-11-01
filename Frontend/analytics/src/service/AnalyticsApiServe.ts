import { ApiClient } from 'shell/api';
import type { Analytics } from '../assets/analytics/Analytics';

// interface PaginatedResponse<T> {
//   items: T[];
//   totalPages: number;
// }

class AnalyticsApiService {
    private readonly url = 'Analytics';

    async getAll(params?: { page?: number }): Promise<Analytics[]> {
        return ApiClient.get(`${this.url}/All`, { params });
    }

    async get(id: string): Promise<Analytics> {
        return ApiClient.get(`${this.url}/${id}`);
    }

    async create(data: Partial<Analytics>): Promise<Analytics> {
        return ApiClient.post(`${this.url}/Add`, data);
    }

    async update(id: string, data: Partial<Analytics>): Promise<Analytics> {
        return ApiClient.put(`${this.url}/Change/${id}`, data);
    }

    async delete(id: string): Promise<void> {
        return ApiClient.delete(`${this.url}/Delete/${id}`);
    }
}

export default new AnalyticsApiService();