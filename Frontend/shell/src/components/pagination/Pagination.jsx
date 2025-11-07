import { Pagination as BootstrapPagination } from 'react-bootstrap';
import usePagination from './PaginationHook';

const Pagination = ({ totalPages }) => {
    const { currentPage, setPage } = usePagination();

    if (totalPages <= 1) return null;

    const handlePageChange = (page) => {
        if (page < 1 || page > totalPages) return;
        setPage(page);
    };

    const renderItems = () => {
        const items = [];

        if (currentPage > 2) {
            items.push(
                <BootstrapPagination.Item
                    key={1}
                    onClick={() => handlePageChange(1)}
                >
                    1
                </BootstrapPagination.Item>
            );

            if (currentPage > 3) {
                items.push(<BootstrapPagination.Ellipsis key="start-ellipsis" disabled />);
            }
        }

        if (currentPage > 1) {
            items.push(
                <BootstrapPagination.Item
                    key={currentPage - 1}
                    onClick={() => handlePageChange(currentPage - 1)}
                >
                    {currentPage - 1}
                </BootstrapPagination.Item>
            );
        }

        items.push(
            <BootstrapPagination.Item key={currentPage} active>
                {currentPage}
            </BootstrapPagination.Item>
        );

        if (currentPage < totalPages) {
            items.push(
                <BootstrapPagination.Item
                    key={currentPage + 1}
                    onClick={() => handlePageChange(currentPage + 1)}
                >
                    {currentPage + 1}
                </BootstrapPagination.Item>
            );
        }

        if (currentPage < totalPages - 1) {
            if (currentPage < totalPages - 2) {
                items.push(<BootstrapPagination.Ellipsis key="end-ellipsis" disabled />);
            }

            items.push(
                <BootstrapPagination.Item
                    key={totalPages}
                    onClick={() => handlePageChange(totalPages)}
                >
                    {totalPages}
                </BootstrapPagination.Item>
            );
        }

        return items;
    };

    return (
        <div className="d-flex justify-content-center mt-3">
            <BootstrapPagination>
                <BootstrapPagination.Prev
                    onClick={() => handlePageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                />
                {renderItems()}
                <BootstrapPagination.Next
                    onClick={() => handlePageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                />
            </BootstrapPagination>
        </div>
    );
};

export default Pagination;
