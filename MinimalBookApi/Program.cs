using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var books = new List<Book>
{
    new Book { Id = 1, Title = "The Pharaoh's Legacy", Author = "Ahmed El-Masri" },
    new Book { Id = 2, Title = "Mysteries of the Nile", Author = "Sara Abdel-Rahman" },
    new Book { Id = 3, Title = "Secrets of Ancient Egypt", Author = "Omar Hassan" },
    new Book { Id = 4, Title = "Legends of the Pyramids", Author = "Nadia Farouk" }
};

// GET all books
app.MapGet("/book", () =>
{
    return Results.Ok(books);
});

// GET a single book by ID
app.MapGet("/book/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book == null)
    {
        return Results.NotFound("Sorry, this book doesn't exist.");
    }

    return Results.Ok(book);
});

// POST a new book
app.MapPost("/book", (Book book) =>
{
    var newId = books.Max(b => b.Id) + 1;
    book.Id = newId;
    books.Add(book);
    return Results.Created($"/book/{book.Id}", book);
});

// PUT to update a book
app.MapPut("/book/{id}", (int id, Book updatedBook) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book == null)
    {
        return Results.NotFound("Sorry, this book doesn't exist.");
    }

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    return Results.Ok(book);
});

// DELETE a book
app.MapDelete("/book/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book == null)
    {
        return Results.NotFound("Sorry, this book doesn't exist.");
    }

    books.Remove(book);
    return Results.NoContent();
});

app.Run();

class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
}

