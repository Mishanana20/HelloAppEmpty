using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MovieSqlExpress;

internal class SelectMovies
{
    /// <summary>
    /// На вход поступает запрос имеющий 3 параметра:
    /// Название фильма, Имя актера и Жанр фильма
    /// </summary>
    /// <returns>
    /// На выходе список фильмов в виде обектов List(Movie), которые в дальнейшем десериализируем в Json
    /// </returns>
    public static List<Movie> FindMovies(string? filmName, string? actorName, string? genreName)
    {

        using (movie_dbContext db = new movie_dbContext())
        {
            bool isAvalaible = db.Database.CanConnect();
            if (!isAvalaible)
            {
                Console.WriteLine("база данных недоступна");
                return(null);
            }
            else Console.WriteLine("база данных доступна");
        }
        using (movie_dbContext db = new movie_dbContext())
        {
           // Console.WriteLine(filmName + "ЭТО НАЗВАНИЕ ФИЛЬМА ИЗ ПОСКА!\n\n\n");
                var something = (db.Movies.AsNoTracking()
                .Include(a => a.Genres)
                .Include(a => a.Actors).Where(a => EF.Functions.Like(a.Name, $"%{filmName}%"))).ToList();
                if (something.Any())  //if (something != null)
                {
                    Console.WriteLine("Выборка что-то имеет");
                    foreach (var q in something)
                    {
                        Console.WriteLine($"*** {q.Name}");
                        foreach (var a in q.Genres)
                        {
                            Console.WriteLine($"{a.Name}");
                        }
                        foreach (var a in q.Actors)
                        {
                            Console.WriteLine($"{a.FirstName} {a.SecondName}");
                        }
                    }
                }
                else Console.WriteLine("выборка пуста");

                var somethingActors = db.Actors.AsNoTracking()
                                      .Include(a => a.Movies)
                                      .Where(a => EF.Functions.Like(a.FirstName, $"%{actorName}%"));
                if (somethingActors.Any())  //if (something != null)
                {
                    Console.WriteLine("Выборка имеет актеров");
                    foreach (var q in somethingActors)
                    {
                        Console.WriteLine($"*** {q.FirstName} {q.SecondName}");
                        foreach (var a in q.Movies)
                        {
                            Console.WriteLine($"{a.Name}");
                        }

                    }
                }
                else Console.WriteLine("выборка пуста");

                var somethingGenre = db.Genres.AsNoTracking()
                                      .Include(a => a.Movies)
                                      .Where(a => EF.Functions.Like(a.Name, $"%{genreName}%"));
                if (somethingActors.Any())  //if (something != null)
                {
                    Console.WriteLine("Выборка имеет эти жанры");
                    foreach (var q in somethingGenre)
                    {
                        Console.WriteLine($"*** {q.Name}");
                        foreach (var a in q.Movies)
                        {
                            Console.WriteLine($"{a.Name}");
                        }

                    }
                }
                else Console.WriteLine("выорка пуста");

                List<Movie> movies = new List<Movie>();
                foreach (var movie in something)
                {
                    bool isHasActor = false, isHasGenre = false;
                    foreach (var actor in movie.Actors)
                    {
                        if (actor.FirstName.ToLower().Contains(actorName!.ToLower()))
                        {
                            isHasActor = true;
                        }
                    }
                    foreach (var genre in movie.Genres)
                    {
                        if (genre.Name.ToLower().Contains(genreName!.ToLower()))
                        {
                            isHasGenre = true;
                        }
                    }
                    if (isHasGenre == true && isHasActor == true)
                    {
                        movies.Add(movie);
                    }
                }
                //если фильмы имеют Any(от фильмов актеров), то добавить
                movies.Distinct();
                if (movies.Any())  //if (something != null)
                {
                    Console.WriteLine(@"/\/\/\/\/\/\/\/\/\/\/\/\/\/\");

                    Console.WriteLine("Результаты поиска:");
                    foreach (var m in movies)
                    {
                        Console.WriteLine($"*** {m.Name}");
                        Console.WriteLine("Актеры:");
                        foreach (var a in m.Actors)
                        {
                            Console.WriteLine($"{a.FirstName} {a.SecondName}");
                        }
                        Console.WriteLine("Жанры:");
                        foreach (var a in m.Genres)
                        {
                            Console.WriteLine($"{a.Name}");
                        }
                    }
                }
                else { Console.WriteLine("По данному запросу ничего не найдено"); }

            return (movies);
        }
    }
}