import React, { useEffect, useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import axios from 'axios';
import { useAuthContext } from '../AuthContext';
import JokeBox from '../Components/JokesBox';

const Jokes = () => {
    const [jokes, setJokes] = useState([]);
    useEffect(() => {
        const getJokes = async () => {
            const { data } = await axios.get('/api/joke/getjokes');
            setJokes(data);
        }

        getJokes();

    }, []);


    return <
        div id="root" >
        <div>
            <div className="container">
                <div>
                    {jokes && jokes.map((m, i) =>
                        <JokeBox
                            key={i}
                            Joke={m}
                        />)}
                </div>
            </div>
        </div>
    </div >
}
export default Jokes;