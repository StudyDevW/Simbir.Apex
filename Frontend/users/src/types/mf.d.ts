declare module 'shell/api' {
    import type { AxiosInstance } from 'axios';
    export const ApiClient: AxiosInstance;
}

declare module 'shell/pagination/Pagination' {
    const Pagination: React.ComponentType<{ totalPages: number }>;
    export default Pagination;
}

declare module 'shell/pagination/usePagination' {
    const usePagination: () => {
        currentPage: number;
        setPage: (page: number) => void;
    };
    export default usePagination;
}