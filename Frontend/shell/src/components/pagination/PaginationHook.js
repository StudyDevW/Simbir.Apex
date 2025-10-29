import { useSearchParams } from 'react-router-dom';

const usePagination = () => {
    const [searchParams, setSearchParams] = useSearchParams();
    const currentPage = parseInt(searchParams.get('page')) || 1;

    const setPage = (newPage) => {
        if (newPage < 1) return;

        const params = Object.fromEntries(searchParams.entries());
        params.page = newPage.toString();
        setSearchParams(params);
    };

    return { currentPage, setPage };
};

export default usePagination;
