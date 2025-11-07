import { useEffect, useState } from "react";
import UsersApiService from "../service/UsersApiServe";
import type { User } from "../users/Users";

const useUsers = (page = 1) => {
    const [users, setUsers] = useState<User[]>([]);
    // const [totalPages, setTotalPages] = useState(1);
    const [usersRefresh, setUsersRefresh] = useState(false);

    const handleUsersChange = () => setUsersRefresh((prev) => !prev);

    const getUsers = async () => {
        try {
            const response = await UsersApiService.getAll({ page });

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
