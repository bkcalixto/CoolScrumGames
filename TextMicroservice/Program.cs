using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build(); //This page is what actually creates the API endpoint with the text

//The CORS allows the request to actually get through to the API endpoint. No idea why it's necessary but it didn't work without it.
app.UseCors(builder =>
{
    builder
        .WithOrigins("https://localhost:7035")
        .AllowAnyHeader()
        .AllowAnyMethod();
});


string getText() //This method is what pulls the data from the text file.
{
    string text = "";
    try
    {
        // Sets the path to the text file that you want to read
        string filePath = "AboutUs.txt";
        // Reads the content of the text file
        text = System.IO.File.ReadAllText(filePath);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
    return text;
}

app.MapGet("/AboutUs", () =>
{
    var text = new AboutUsText(getText());
    return text;
})
.WithName("GetAboutUs");

app.Run();


internal record AboutUsText(string text)
{
    string t = text;
}