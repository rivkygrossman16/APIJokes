import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import axios from 'axios';
import { useAuthContext } from '../AuthContext';


const JokeBox = ({ Joke }) => {

    const history = useHistory();
    const user = useAuthContext();
    const { setup, punchline, id, likes, dislikes } = Joke;

    return (
        <div>
            <div className="card card-body bg-light mb-3">
                <h5>{setup}</h5>
                <h5>{punchline}</h5>
                <span>Likes: {likes}</span>
                <br />
                <span>Dislikes: {dislikes}</span>
              
            </div>

        </div>
    );

}
export default JokeBox;