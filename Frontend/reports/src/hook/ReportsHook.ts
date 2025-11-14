import { useEffect, useState } from "react";
import ReportsApiService from "../service/ReportsApiService";
import type { Reports } from "../reports/Reports";

const useUsers = (page = 1) => {
    const [users, setUsers] = useState<Reports[]>([]);
    // const [totalPages, setTotalPages] = useState(1);
    const [usersRefresh, setUsersRefresh] = useState(false);

    const handleUsersChange = () => setUsersRefresh((prev) => !prev);

    const getUsers = async () => {
        try {
            const response = await ReportsApiService.getAll({ page });

            setUsers(response ?? []);
            // setTotalPages(response.totalPages ?? 1);
        } catch (e) {
            console.error("Failed to fetch users:", e);
            setUsers([]);
            // setTotalPages(1);
        }
    };

    useEffect(() => {
        getUsers();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, usersRefresh]);

    return {
        users,
        // totalPages,
        handleUsersChange,
    };
};

export default useUsers;
