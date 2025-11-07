import { ApiClient } from '../../api/ApiClient';

class AuthApiService {
    url = 'Auth';

    async login(body) {
        return ApiClient.post(`${this.url}/SignIn`, body);
    }
}

export default new AuthApiService();