var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient("UserServiceClient", client =>
{
    client.BaseAddress = new Uri("http://user-service:8080"); // Docker 
});

var app = builder.Build();

var products = new List<Product>
{
    new Product(101, "Зөөврийн компьютер", 1),
    new Product(102, "Гар утас", 2)
};

// Энэ endpoint нь барааг авахдаа эзэмшигчийн мэдээллийг UserService-ээс татаж нэгтгэнэ
app.MapGet("/api/products-with-users", async (IHttpClientFactory clientFactory) =>
{
    var httpClient = clientFactory.CreateClient("UserServiceClient");
    var resultList = new List<object>();

    foreach (var product in products)
    {
        // UserService-ийн api/users/{id} endpoint-ийг дуудаж байна
        var userResponse = await httpClient.GetAsync($"/api/users/{product.UserId}");
        
        string uName = "Үл мэдэгдэх хэрэглэгч";
        if (userResponse.IsSuccessStatusCode)
        {
            var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();
            uName = user?.Name ?? uName;
        }

        resultList.Add(new
        {
            product.Id,
            product.Name,
            OwnerName = uName
        });
    }

    return Results.Ok(resultList);
});

app.Run();

public record Product(int Id, string Name, int UserId);
public record UserDto(int Id, string Name, string Email); // UserService-ээс ирэх датаг хүлээж авах DTO