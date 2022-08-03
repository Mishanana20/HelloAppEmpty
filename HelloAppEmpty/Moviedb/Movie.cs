using System;
using System.Collections.Generic;

namespace MovieSqlExpress
{
    public partial class Movie
    {
        public Movie()
        {
            Actors = new HashSet<Actor>();
            Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
