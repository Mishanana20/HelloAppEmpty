using System;
using System.Collections.Generic;

namespace MovieSqlExpress
{
    public partial class Actor
    {
        public Actor()
        {
            Movies = new HashSet<Movie>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
