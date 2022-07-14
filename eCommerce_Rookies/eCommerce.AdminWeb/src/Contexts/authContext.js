import React, { createContext, useEffect, useReducer } from 'react';
import jwtDecode from 'jwt-decode';
import axios from '../axios';
import { authRoles } from '../Auth/AuthRole';

const initialState = {
  isAuthenticated: false,
  isInitialised: false,
  userName: null,
};

const isValidToken = (accessToken) => {
  if (!accessToken) {
    return false;
  }

  const decodedToken = jwtDecode(accessToken);
  const currentTime = Date.now() / 1000;
  return decodedToken.exp > currentTime;
};

const setSession = (accessToken, userName) => {
  if (accessToken) {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('userName', userName);
    // localStorage.setItem('isInRole', isInRole);
    axios.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
  } else {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('userName');
    // localStorage.removeItem('isInRole');
    delete axios.defaults.headers.common.Authorization;
  }
};

const reducer = (state, action) => {
  switch (action.type) {
    case 'INIT': {
      const { isAuthenticated, userName } = action.payload;

      return {
        ...state,
        isAuthenticated,
        isInitialised: true,
        userName,
      };
    }
    case 'LOGIN': {
      const { userName } = action.payload;
      // const { isInRole } = action.payload;
      return {
        ...state,
        isAuthenticated: true,
        userName,
        // isInRole,
      };
    }
    case 'LOGOUT': {
      return {
        ...state,
        isAuthenticated: false,
        userName: null,
      };
    }
    case 'REGISTER': {
      const { userName } = action.payload;

      return {
        ...state,
        isAuthenticated: true,
        userName,
      };
    }
    default: {
      return { ...state };
    }
  }
};

const AuthContext = createContext({
  ...initialState,
  method: 'JWT',
  login: () => Promise.resolve(),
  logout: () => {},
  register: () => Promise.resolve(),
});

export const AuthProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const login = async (email, password) => {
    const response = await axios.post('Auth/Login', {
      email,
      password,
    });
    const { userName,accessToken, refreshToken, expiration} = response;
    console.log(accessToken);
    console.log(userName); 
    setSession(accessToken, userName);

    dispatch({
      type: 'LOGIN',
      payload: {
        userName,
        // isInRole,
      },
    });
  };

  const register = async (email, userName, password) => {
    const response = await axios.post('/api/auth/register', {
      email,
      userName,
      password,
    });

    const { accessToken, user } = response.data;

    setSession(accessToken);

    dispatch({
      type: 'REGISTER',
      payload: {
        user,
      },
    });
  };

  const logout = () => {
    setSession(null);
    dispatch({ type: 'LOGOUT' });
  };

  useEffect(() => {
    (async () => {
      try {
        const accessToken = window.localStorage.getItem('accessToken');
        const userName = window.localStorage.getItem('userName');
        // const isInRole = window.localStorage.getItem('isInRole');
        if (accessToken && isValidToken(accessToken)) {
          setSession(accessToken);
          // const response = await axios.get('Users/get-by-user-name/', { userName });
          // const { userName } = response;

          dispatch({
            type: 'INIT',
            payload: {
              isAuthenticated: true,
              // userName,
            },
          });
        } else if (accessToken && isValidToken(accessToken)) {
          dispatch({
            type: 'INIT',
            payload: {
              isAuthenticated: false,
              userName: null,
            },
          });
        } else {
          dispatch({
            type: 'INIT',
            payload: {
              isAuthenticated: false,
              user: null,
            },
          });
        }
      } catch (err) {
        console.error(err);
        dispatch({
          type: 'INIT',
          payload: {
            isAuthenticated: false,
            user: null,
          },
        });
      }
    })();
  }, []);

  // if (!state.isInitialised) {
  //   return <MatxLoading />;
  // }

  return (
    <AuthContext.Provider
      value={{
        ...state,
        method: 'JWT',
        login,
        logout,
        register,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;