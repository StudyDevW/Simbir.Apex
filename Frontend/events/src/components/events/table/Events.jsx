import { useState } from 'react';
import useEvents from '../hooks/EventsHook.js';
import EventsTable from './EventsTable.jsx';
import EventsTableRow from './EventsTableRow.jsx';
import { Button, Modal } from 'react-bootstrap';
import PseudoQueryModal from '../form/PseudoQueryModal.jsx';
import { Funnel } from "react-bootstrap-icons";
import Pagination from 'shell/pagination/Pagination';
import usePagination from 'shell/pagination/usePagination';
import useEventsFiltersForm from '../hooks/EventsFiltersHook.js';
import EventsFiltersForm from '../form/EventsFiltersForm.jsx'

const Events = () => {
    const {
        filters,
        setFilters,
        handleFiltersChange,
        showFiltersForm,
        handleShowFiltersForm,
        handleCloseFiltersForm,
        applyFilters,
        resetFilters,
        clearFilters,
    } = useEventsFiltersForm();

    const { currentPage } = usePagination();
    const { events, totalPages, handleEventsChange } = useEvents(filters, currentPage);

    const [showPseudo, setShowPseudo] = useState(false);

    const handleApplyPseudo = (parsedFilters) => {
        clearFilters();
        setFilters(parsedFilters);
        applyFilters(parsedFilters);
        handleEventsChange();
    };

    return (
        <>
            <div className="d-flex mb-2">
                <Button variant="outline-dark" className="me-2" onClick={handleShowFiltersForm}>
                    <Funnel />
                </Button>
                <Button variant="outline-dark" onClick={() => setShowPseudo(true)}>
                    &lt;/&gt;
                </Button>
            </div>

            <Modal show={showFiltersForm} onHide={handleCloseFiltersForm} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Фильтры событий</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <EventsFiltersForm filters={filters} handleChange={handleFiltersChange} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => {resetFilters(); handleEventsChange();}}>Очистить</Button>
                    <Button variant="primary" onClick={() => {applyFilters(); handleEventsChange();}}>Применить</Button>
                </Modal.Footer>
            </Modal>

            <PseudoQueryModal
                show={showPseudo}
                handleClose={() => setShowPseudo(false)}
                applyPseudoQuery={handleApplyPseudo}
            />

            <EventsTable>
                {events.map((event) => (
                    <EventsTableRow key={event.id} event={event} />
                ))}
            </EventsTable>

            <Pagination totalPages={totalPages} />
        </>
    );
};

export default Events;
