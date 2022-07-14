import useAuth from '../hooks/useAuthContext';
import { Navigate, useLocation } from 'react-router-dom';

const AuthGuard = ({ children }) => {
  let {
    isAuthenticated,
  } = useAuth();
  const { pathname } = useLocation();


  let authenticated = isAuthenticated;

  return (
    <>
      {authenticated ? (
        children
      ) : (
        <Navigate replace to="/login" state={{ from: pathname }} />
      )}
    </>
  );
};

export default AuthGuard;