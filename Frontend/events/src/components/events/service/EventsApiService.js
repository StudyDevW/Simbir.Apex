import { ApiClient } from 'shell/api';

class EventsApiService {
    url = 'events';

    async getAll(params = {}) {
        return ApiClient.get(this.url, { params });
    }

    async getDevices() {
        return ApiClient.get('devices');
    }
}

export default new EventsApiService();
