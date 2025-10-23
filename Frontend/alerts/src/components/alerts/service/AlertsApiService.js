import { ApiClient } from 'shell/api';

class AlertsApiService {
    url = 'alerts';

    async getAll(params = {}) {
        return ApiClient.get(this.url, { params });
    }

    async get(id) {
        return ApiClient.get(`${this.url}/${id}`);
    }

    async update(id, body) {
        return ApiClient.put(`${this.url}/${id}`, body)
    }

    async getHostnames() {
        return ApiClient.get(`hostnames`)
    }

    async getUsers() {
        return ApiClient.get(`users`)
    }

    async getRules() {
        return ApiClient.get(`rules`)
    }
}

export default new AlertsApiService();