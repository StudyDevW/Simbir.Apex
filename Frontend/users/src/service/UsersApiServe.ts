import { ApiClient } from 'shell/api';
import type { User } from '../users/Users';

// interface PaginatedResponse<T> {
//   items: T[];
//   totalPages: number;
// }

class UsersApiService {
    private readonly url = 'User';

    async getAll(params?: { page?: number }): Promise<User[]> {
        return ApiClient.get(`${this.url}/All`, { params });
    }

    async get(id: string): Promise<User> {
        return ApiClient.get(`${this.url}/${id}`);
    }

    async create(data: Partial<User>): Promise<User> {
        return ApiClient.post(`${this.url}/Add`, data);
    }

    async update(id: string, data: Partial<User>): Promise<User> {
        return ApiClient.put(`${this.url}/Change/${id}`, data);
    }

    async delete(id: string): Promise<void> {
        return ApiClient.delete(`${this.url}/Delete/${id}`);
    }
}

export default new UsersApiService();