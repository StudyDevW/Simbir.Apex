import Users from "users/Users"
import PageTitle from "../components/PageTitle/PageTitle";

const UsersPage = () => {
    return (
        <>
            <PageTitle title="Пользователи"></PageTitle>
            <Users />
        </>
    );
};

export default UsersPage;
