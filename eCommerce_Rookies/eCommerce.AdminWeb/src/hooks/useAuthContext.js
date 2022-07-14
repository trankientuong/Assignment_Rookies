import { useContext } from 'react'
import AuthContext  from '../Contexts/authContext';

const useAuth = () => useContext(AuthContext)

export default useAuth