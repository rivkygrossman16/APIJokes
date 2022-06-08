using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIReact.Data
{
    public class JokeRepo
    {
        private readonly string _connectionString;

        public JokeRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using var ctx = new JokesContext(_connectionString);
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }

            return user;

        }
        public User GetByEmail(string email)
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }
        public List<Joke> GetJokes()
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Jokes.ToList();
        }
        public int GetLikes(int id)
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Likes.Where(s => s.JokeId == id && s.Liked == true).Count();
        }
        public int GetDislikes(int id)
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Likes.Where(s => s.JokeId == id && s.Liked == false).Count();
        }

        public Joke AddJoke(Joke joke)
        {
            using var ctx = new JokesContext(_connectionString);
            joke.Id = 0;
            if (!ctx.Jokes.Any(a => a.Setup == joke.Setup) && !ctx.Jokes.Any(a => a.Punchline == joke.Punchline))
            {
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
            joke = ctx.Jokes.Where(a => a.Setup == joke.Setup && a.Punchline == joke.Punchline).FirstOrDefault();

            return joke;
              
        }

        public void LikeOrDislike(UserLikedJokes  ulj)
        {
            using var ctx = new JokesContext(_connectionString);
            if (!ctx.Likes.Any(a=>a.UserId==ulj.UserId&& a.JokeId==ulj.JokeId))
            {
                ctx.Likes.Add(ulj);
                ctx.SaveChanges();
            }
            else
            {
                ctx.Likes.Update(ulj);
                ctx.SaveChanges();
            }


        }


        public UserLikedJokes GetLikeorDislike(int id, int userId)
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Likes.FirstOrDefault(a => a.UserId == userId && a.JokeId == id);

        }
    }
}
