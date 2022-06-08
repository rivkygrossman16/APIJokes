using APIReact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReact.Web.Models
{
    public class JokesWithLikesDislikes:Joke
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
