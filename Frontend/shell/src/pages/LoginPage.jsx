import AuthForm from "../auth/form/AuthForm";
import PageTitle from "../components/PageTitle/PageTitle";
const LoginPage = () => {
    return (
        <>
            <PageTitle title="Вход"></PageTitle>
            <AuthForm />
        </>
    );
};

export default LoginPage;