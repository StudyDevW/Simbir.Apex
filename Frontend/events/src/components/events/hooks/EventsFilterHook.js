import { useSearchParams } from 'react-router-dom';
import useDevices from './DevicesHook';

const useFilter = (filterName) => {
    const [searchParams] = useSearchParams();
    return searchParams.get(filterName) || '';
};


const useDeviceFilter = () => {
    const { devices } = useDevices();
    const [searchParams] = useSearchParams();
    return {
        devices,
        currentDeviceFilter: searchParams.get('device') || ''
    };
};

export { useFilter, useDeviceFilter };
