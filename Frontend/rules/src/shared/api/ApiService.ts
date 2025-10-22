import { ApiClient } from "./ApiClient";

class ApiService<T> {
  private url: string;

  constructor(url: string) {
    this.url = url;
  }

  async getAll(expand?: string): Promise<T[]> {
    const data: T[] = await ApiClient.get(`${this.url}${expand || ""}`);
    return data;
  }

  async get(id: string | number, expand?: string): Promise<T> {
    const data: T = await ApiClient.get(`${this.url}/${id}${expand || ""}`);
    return data;
  }

  async create(body: Partial<T>): Promise<T> {
    const data: T = await ApiClient.post(this.url, body);
    return data;
  }

  async update(id: string | number, body: Partial<T>): Promise<T> {
    const data: T = await ApiClient.put(`${this.url}/${id}`, body);
    return data;
  }

  async delete(id: string | number): Promise<void> {
    await ApiClient.delete(`${this.url}/${id}`);
  }
}

export default ApiService;
