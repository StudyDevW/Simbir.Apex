import { useSearchParams } from 'react-router-dom';
import { useState } from 'react';

const useEventsFiltersForm = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const initialFilters = {
        category: searchParams.get('category') || '',
        device: searchParams.get('device') || '',
        isLan: searchParams.get('isLan') || '',
        startDate: searchParams.get('startDate') || '',
        endDate: searchParams.get('endDate') || ''
    };

    const [filters, setFilters] = useState(initialFilters);
    const [showFilters, setShowFilters] = useState(false);

    const handleChange = (event) => {
        const { name, value, type, checked } = event.target;
        setFilters((prev) => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleCloseFilters = () => setShowFilters(false);

    const handleShowFilters = () => {
        const currentFilters = {
            category: searchParams.get('category') || '',
            device: searchParams.get('device') || '',
            isLan: searchParams.get('isLan') || '',
            startDate: searchParams.get('startDate') || '',
            endDate: searchParams.get('endDate') || ''
        };
        setFilters(currentFilters);
        setShowFilters(true);
    };

    const applyFilters = (customFilters = filters) => {
        const next = new URLSearchParams();

        Object.entries(customFilters).forEach(([key, value]) => {
            if (value) next.set(key, value);
            else next.delete(key);
        });

        setSearchParams(next);
        handleCloseFilters();
    };

    const clearFilters = () => {
        setSearchParams({});
        setFilters({
            category: '',
            device: '',
            isLan: '',
            startDate: '',
            endDate: ''
        });
    };

    const resetFilters = () => {
        clearFilters();
        handleCloseFilters();
    };

    return {
        filters,
        setFilters,
        handleFiltersChange: handleChange,
        showFiltersForm: showFilters,
        handleShowFiltersForm: handleShowFilters,
        handleCloseFiltersForm: handleCloseFilters,
        applyFilters,
        resetFilters,
        clearFilters
    };
};

export default useEventsFiltersForm;
