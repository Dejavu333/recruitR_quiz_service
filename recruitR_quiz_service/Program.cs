using MongoDB.Driver;
using recruitR_quiz_service.Model.Repository;
using recruitR_quiz_service.quiz.Repository;

namespace recruitR_quiz_service;
public static partial class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        addServices(builder);

        WebApplication app = builder.Build();
        useServices(app);
        mapRoutes(app);

        app.Run();
    }

    public static void addServices(WebApplicationBuilder builder)
    {
        //builder.Services.AddAuthentication();
        //builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        /* Loads MongoDB configuration from appsettings.json */
        var mongoConfig = builder.Configuration.GetSection("MongoDB").Get<MongoConfiguration>();
        /* Registers MongoDB client as a singleton */
        builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConfig.connectionString));
        builder.Services.AddSingleton<IMongoDatabase>((serviceProvider) =>
        {
            IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoConfig.databaseName);
        });
        builder.Services.AddSingleton<IQuizRepository, MongoQuizRepository>();
    }

    public static void useServices(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseHttpsRedirection(); // dotnet dev-certs https --trust
        //app.UseAuthentication();
        //app.UseAuthorization();
    }
}

public class MongoConfiguration
{
    public string connectionString { get; set; }
    public string databaseName { get; set; }
}