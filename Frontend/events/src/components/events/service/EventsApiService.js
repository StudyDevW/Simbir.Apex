import { ApiClient } from 'shell/api';

class EventsApiService {
    url = 'Events';

    async getAll(params = {}) {
        return ApiClient.get(`${this.url}/All`, { params });
    }

    async getDevices() {
        return ApiClient.get('devices');
    }
}

export default new EventsApiService();
