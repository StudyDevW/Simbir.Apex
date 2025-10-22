import { useEffect, useState } from 'react';
import EventsApiService from '../service/EventsApiService';

const useEvents = ({ categoryFilter, deviceFilter, lanFilter, startDateFilter, endDateFilter }) => {
    const [events, setEvents] = useState([]);
    const [eventsRefresh, setEventsRefresh] = useState(false);
    const handleEventsChange = () => setEventsRefresh(!eventsRefresh);
    const clearFilters = () => {categoryFilter, deviceFilter, lanFilter, startDateFilter, endDateFilter = null;}

    const getEvents = async () => {
        const params = {};

        if (categoryFilter) params.category = categoryFilter;
        if (deviceFilter) params.device = deviceFilter;
        if (lanFilter) params.isLan = lanFilter;
        if (startDateFilter) params.startDate = startDateFilter;
        if (endDateFilter) params.endDate = endDateFilter;

        const data = await EventsApiService.getAll(params);
        setEvents(data ?? []);
    };

    useEffect(() => {
        getEvents();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [categoryFilter, deviceFilter, lanFilter, startDateFilter, endDateFilter, eventsRefresh]);

    return {
        events,
        handleEventsChange,
        clearFilters
    };
};

export default useEvents;
