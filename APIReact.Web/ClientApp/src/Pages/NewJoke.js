import React, { useEffect, useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import axios from 'axios';
import { useAuthContext } from '../AuthContext';
import JokeBox from '../Components/JokesBox';

const NewJoke = () => {
    const [joke, setJoke] = useState([]);
    const [jokeId, setId] = useState(0);
    const [disableLike, setDisableLike] = useState(false);
    const [disableDislike, setDisableDislike] = useState(false);
    const [likes, setLikes] = useState('');
    const [dislikes, setdislikes] = useState('');
    const { user } = useAuthContext();
    const { setup, punchline, id/*, likes, dislikes*/ } = joke;
    useEffect(() => {
        const getJoke = async () => {
            const { data } = await axios.get('/api/joke/getjoke');
            setJoke(data);
            setId(data.id);
            console.log(jokeId);
        }

        getJoke();

    }, []);

    const getJoke = async () => {
        const { data } = await axios.get('/api/joke/getjoke');
        setJoke(data);
        setId(data.id);
        console.log(jokeId);
    }

    const likeJoke = async () => {
        var theId = id;
        await axios.post(`/api/joke/likejoke?id=${id}`);
        const { data } = await axios.get(`/api/joke/getlikeordislike?id=${jokeId}`);
    }

    const dislikeJoke = async () => {
        await axios.post(`/api/joke/dislikejoke?id=${id}`);
        const { data } = await axios.get(`/api/joke/getlikeordislike?id=${jokeId}`);
    }

    const getLikes = async () => {
        const { data } = await axios.get(`/api/joke/getlikes?id=${id}`);
        console.log(data);
        setLikes(data);
    }
    const getdislikes = async () => {
        const { data } = await axios.get(`/api/joke/getdislikes?id=${id}`);
        setdislikes(data);
    }

    const interval = setInterval(async () => {
        if (jokeId) {
            const { data } = await axios.get(`/api/joke/getlikeordislike?id=${jokeId}`);
            //const currentTime = new Date().toLocaleTimeString();
            //const time = data.time;
            ///* const thisTime = time.getHours();*/
            setDisableLike(data.liked);
            setDisableDislike(!data.liked);
            console.log('went it');
        }
        getLikes();
        getdislikes();

          

    }, 500);


    return (
        <div>
            <div className="card card-body bg-light mb-3">
                <h5>{setup}</h5>
                <h5>{punchline}</h5>
                {!user && <div>
                    <Link to="/login">Login to your account to like/dislike this joke</Link>
                </div>}
                <span>Likes: {likes}</span>
                <br />
                <span>Dislikes: {dislikes}</span>
                {user && <div>
                    <button className="btn btn-primary" disabled={disableLike} onClick={likeJoke}>Like</button>
                    <button className="btn btn-danger" disabled={disableDislike} onClick={dislikeJoke}>Dislike</button>
                </div>}
                <h4>
                    <button onClick={() => window.location.reload(false)} className="btn btn-link">Refresh</button>
                </h4>
            </div>

        </div>
    );
}
export default NewJoke;