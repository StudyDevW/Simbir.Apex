import { useSearchParams } from 'react-router-dom';
import { useState } from 'react';

const useAlertsFiltersForm = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const initialFilters = {
        ruleId: searchParams.get('ruleId') || '',
        assignedToId: searchParams.get('assignedToId') || '',
        hostname: searchParams.get('hostname') || '',
        status: searchParams.get('status') || '',
        severity: searchParams.get('severity') || '',
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
            ruleId: searchParams.get('ruleId') || '',
            assignedToId: searchParams.get('assignedToId') || '',
            hostname: searchParams.get('hostname') || '',
            status: searchParams.get('status') || '',
            severity: searchParams.get('severity') || '',
            startDate: searchParams.get('startDate') || '',
            endDate: searchParams.get('endDate') || ''
        };
        setFilters(currentFilters);
        setShowFilters(true);
    };

    const applyFilters = () => {
        const next = new URLSearchParams(searchParams);

        Object.entries(filters).forEach(([key, value]) => {
            if (value) next.set(key, value);
            else next.delete(key);
        });

        setSearchParams(next);
        handleCloseFilters();
    };

    const clearFilters = () => {
        setSearchParams({});
        setFilters({
            ruleId: '',
            assignedToId: '',
            hostname: '',
            status: '',
            severity: '',
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
        handleFiltersChange: handleChange,
        showFiltersForm: showFilters,
        handleShowFiltersForm: handleShowFilters,
        handleCloseFiltersForm: handleCloseFilters,
        applyFilters,
        resetFilters
    };
};

export default useAlertsFiltersForm;
