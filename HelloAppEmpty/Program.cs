using MovieSqlExpress;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

List<Film> movies = new List<Film> {};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    //await GetAllPeople(response);
   // movies = SelectMovies.FindMovies("", "", "");
    if (path == "/api/users" && request.Method == "GET")
    {
        //await GetAllPeople(response);
    }
    else if (path == "/api/users" && request.Method == "POST")
    {
        await FindFilms(response, request);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

// получение всех пользователей
async Task GetAllPeople(HttpResponse response)
{
    movies.Clear();
    //нужна функция их получения
    foreach (var movie in SelectMovies.FindMovies("", "", ""))
    {
        movies.Add(new Film { Id = movie.Id, Name = movie.Name, Description = movie.Description });
    }
    await response.WriteAsJsonAsync(movies);
}

async Task FindFilms(HttpResponse response, HttpRequest request)
{
    try
    {
        movies.Clear();
        //movies = null!;
        // получаем данные пользователя
        FindFilm parametersFindFilms = await request.ReadFromJsonAsync<FindFilm>();
        //Console.WriteLine(parametersFindFilms.Name + "'NJ ;T ABKMV \n !");
       
        foreach (var movie in SelectMovies.FindMovies(parametersFindFilms.Name, parametersFindFilms.Actor, parametersFindFilms.Genre))
        {
            movies.Add(new Film { Id = movie.Id, Name = movie.Name, Description = movie.Description });
        }
        await response.WriteAsJsonAsync(movies);
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}

public class Film 
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public class FindFilm
{
    public string? Name { get; set; } = "";
    public string? Actor { get; set; } = "";
    public string? Genre { get; set; } = "";
}