import { useEffect, useState } from 'react';
import EventsApiService from '../service/EventsApiService';

const useEvents = (filters = {}, page = 1) => {
    const [events, setEvents] = useState([]);
    const [eventsRefresh, setEventsRefresh] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const handleEventsChange = () => setEventsRefresh(!eventsRefresh);

    const getEvents = async () => {
        const params = Object.fromEntries(
            Object.entries(filters).filter(
                ([, value]) => value !== '' && value !== null && value !== undefined
            )
        );

        params.page = page;

        const response = await EventsApiService.getAll(params);

        //TODO: заменить на: setEvents(response.items ?? []);
        setEvents(response ?? []);
        setTotalPages(response.totalPages ?? 1);
    };

    useEffect(() => {
        getEvents();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, eventsRefresh]);

    return {
        events,
        handleEventsChange,
        totalPages
    };
};

export default useEvents;
