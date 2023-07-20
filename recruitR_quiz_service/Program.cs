using MongoDB.Bson;
using MongoDB.Driver;

namespace recruitR_quiz_service;
public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


        //builder.Services.AddAuthentication();
        //builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        /* Loads MongoDB configuration from appsettings.json */
        var mongoConfig = builder.Configuration.GetSection("MongoDB").Get<MongoConfiguration>();
        /* Registers MongoDB client as a singleton */
        builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConfig.ConnectionString));
        builder.Services.AddSingleton<IMongoDatabase>((serviceProvider) =>
        {
            IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoConfig.DatabaseName);
        });


        WebApplication app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseHttpsRedirection(); // dotnet dev-certs https --trust
        //app.UseAuthentication();
        //app.UseAuthorization();


        app.MapGet("/", () => { return "hey"; });
        app.MapGet("/getQuiz", getQuiz);
        app.MapGet("/api/items", async (IMongoDatabase db) =>
        {
            var collection = db.GetCollection<QuizQuestion>("your_collection_name");
            var items = await collection.Find(FilterDefinition<QuizQuestion>.Empty).ToListAsync();
            return Results.Ok(items);
        });
        app.MapPost("/api/items", async (IMongoDatabase db, QuizQuestion model) =>
        {
            var collection = db.GetCollection<QuizQuestion>("your_collection_name");
            await collection.InsertOneAsync(model);
            return Results.Created($"/api/items/{model.id}", model);
        });


        app.Run();
    }


    public static string getQuiz()
    {
        return "What is what";
    }
}
public class MongoConfiguration
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
public class QuizQuestion
{
    public ObjectId id { get; set; }
    public string question { get; set; }
}