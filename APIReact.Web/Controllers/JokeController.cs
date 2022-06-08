using APIReact.Data;
using APIReact.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokesAPI.web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : Controller
    {
        private string _connectionString;

        public JokeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        [Route("getjoke")]
        public JokesWithLikesDislikes GetJoke()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var joke = JsonConvert.DeserializeObject<List<Joke>>(json).FirstOrDefault();
            var repo = new JokeRepo(_connectionString);
            Joke NewJoke=repo.AddJoke(joke);
            JokesWithLikesDislikes jwld = new JokesWithLikesDislikes
            {
                Id = NewJoke.Id,
                Punchline = NewJoke.Punchline,
                Type = NewJoke.Type,
                Setup = NewJoke.Setup,
                Likes = repo.GetLikes(joke.Id),
                Dislikes = repo.GetDislikes(joke.Id)
            };
            return jwld;
        }
        [HttpGet]
        [Route("getjokes")]
        public List<JokesWithLikesDislikes> GetJokes()
        {
            var repo = new JokeRepo(_connectionString);
            List<Joke> jokes= repo.GetJokes();
            return jokes.Select(joke => new JokesWithLikesDislikes
            {

                Type = joke.Type,
                Punchline = joke.Punchline,
                Setup = joke.Setup,
                Dislikes = repo.GetDislikes(joke.Id),
                Likes = repo.GetLikes(joke.Id),
                Id = joke.Id

            })
         .ToList();
        }

        [HttpGet]
        [Route("getlikes")]
        public int GetLikes(int id)
        {
            var repo = new JokeRepo(_connectionString);
            return repo.GetLikes(id);
        }

        [HttpGet]
        [Route("getdislikes")]
        public int GetDislikes(int id)
        {
            var repo = new JokeRepo(_connectionString);
            return repo.GetDislikes(id);
        }
        
        [HttpPost]

        [Route("likejoke")]
        public void LikeJoke(int id)
        {
            var repo = new JokeRepo(_connectionString);
            var user=repo.GetByEmail(User.Identity.Name);
            UserLikedJokes ulj = new UserLikedJokes
            {
                JokeId = id,
                Liked = true,
                UserId = user.Id,
                Time=DateTime.Now
            };
             repo.LikeOrDislike(ulj);
        }

        [HttpPost]

        [Route("dislikejoke")]
        public void DislikeJoke(int id)
        {
            var repo = new JokeRepo(_connectionString);
            User user = repo.GetByEmail(User.Identity.Name);
            UserLikedJokes ulj = new UserLikedJokes
            {
                JokeId = id,
                Liked = false,
                UserId = user.Id,
                Time=DateTime.Now
            };
             repo.LikeOrDislike(ulj);
        }

        [HttpGet]
        [Route("getlikeordislike")]
        public UserLikedJokes GetLikeOrDislike(int id)
    {
            var repo = new JokeRepo(_connectionString);
            User user = repo.GetByEmail(User.Identity.Name);
            return repo.GetLikeorDislike(id,user.Id);
        }
    }
}
