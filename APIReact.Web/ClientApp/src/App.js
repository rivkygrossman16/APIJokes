import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './Layout';
import Login from './Pages/Login';
import Signup from './Pages/Signup';
import { AuthContextComponent } from './AuthContext';
import Logout from './Pages/Logout';
import NewJoke from './Pages/NewJoke';
import Jokes from './Pages/Jokes';



export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <AuthContextComponent>
                <Layout>
                    <Route exact path='/signup' component={Signup} />
                    <Route exact path='/login' component={Login} />
                    <Route exact path='/logout' component={Logout} />
                    <Route exact path='/' component={NewJoke} />
                    <Route exact path='/jokes' component={Jokes} />
                </Layout>
            </AuthContextComponent>
        );
    }
}