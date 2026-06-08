var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<User>
{
    new User(1, "Бат", "bat@email.com"),
    new User(2, "Болд", "bold@email.com")
};


app.MapGet("/api/users", () => users);


app.MapGet("/api/users/{id}", (int id) => 
    users.FirstOrDefault(u => u.Id == id) is User user ? Results.Ok(user) : Results.NotFound());

app.Run();

// Модел класс
public record User(int Id, string Name, string Email);