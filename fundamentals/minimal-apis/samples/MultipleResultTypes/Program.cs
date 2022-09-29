using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<List<Book>>((sp) =>
{
    return new(){
    new Book(1, "Testing Dot", "John Doe"),
    new Book(2, "Learn Linq", "Rick Perry"),
    new Book(3, "Generics", "Dalis Chevy"),
    new Book(4, "Testing the Mic", "Bob Tik"),
    new Book(5, "Drop the Dot", "Farmy Lix"),
};
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapGet("/book{id}", Results<Ok<Book>, NotFound> (int id, List<Book> bookList) =>
{
    return bookList.FirstOrDefault((i) => i.Id == id) is Book book
     ? TypedResults.Ok(book)
     : TypedResults.NotFound();
});

app.Run();
record Book(int Id, string Title, string Author);

