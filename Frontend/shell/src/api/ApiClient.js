import axios from 'axios';
import toast from 'react-hot-toast';

export class HttpError extends Error {
    constructor(message = '') {
        super(message);
        this.name = 'HttpError';
        Object.setPrototypeOf(this, new.target.prototype);
        toast.error(message, { id: 'HttpError' });
    }
}

let accessToken = null;

export const TokenService = {
    setAccessToken: (token) => {
        accessToken = token;
    },
    getAccessToken: () => accessToken,
    clear: () => {
        accessToken = null;
    },
};

function responseHandler(response) {
    if (response.status === 200 || response.status === 201) {
        const data = response?.data;
        if (!data) {
            throw new HttpError('API Error. No data!');
        }
        return data;
    }
    throw new HttpError(`API Error! Invalid status code ${response.status}!`);
}

function responseErrorHandler(error) {
    if (error === null) {
        throw new Error('Unrecoverable error!! Error is null!');
    }
    toast.error(error.message, { id: 'AxiosError' });
    return Promise.reject(error.message);
}

export const ApiClient = axios.create({
    baseURL: 'http://localhost:3000/',
    timeout: 3000,
    headers: {
        Accept: 'application/json',
    },
});

ApiClient.interceptors.request.use(
    (config) => {
        const token = TokenService.getAccessToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

ApiClient.interceptors.response.use(responseHandler, responseErrorHandler);
