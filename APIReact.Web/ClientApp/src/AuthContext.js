import React, { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios';

const AuthContext = createContext();

const AuthContextComponent = ({ children }) => {

    const [user, setUser] = useState('');

    useEffect(() => {
        const getUser = async () => {
            const { data } = await axios.get('/api/account/getcurrentuser');
            setUser(data);
        }

        getUser();
    }, []);

    const updateUser = async () => {
        const { data } = await axios.get('/api/account/getcurrentuser');
        setUser(data);
    }


    return <AuthContext.Provider value={{ user, setUser, updateUser }}>
        {children}
    </AuthContext.Provider>

}

const useAuthContext = () => useContext(AuthContext);


export { AuthContextComponent, useAuthContext };