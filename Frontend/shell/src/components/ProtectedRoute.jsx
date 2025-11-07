import { Navigate } from "react-router-dom";
import { TokenService } from "../api/ApiClient";

const ProtectedRoute = ({ children }) => {
  const token = TokenService.getAccessToken();

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default ProtectedRoute;