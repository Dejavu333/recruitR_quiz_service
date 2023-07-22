using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

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
        builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConfig.connectionString));
        builder.Services.AddSingleton<IMongoDatabase>((serviceProvider) =>
        {
            IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoConfig.databaseName);
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

        app.MapGet("/api/quizQuestions/v1", async (IMongoDatabase db) =>
        {
            var collection = db.GetCollection<QuizQuestion>("your_collection_name");
            var items = await collection.Find(FilterDefinition<QuizQuestion>.Empty).ToListAsync();
            return Results.Ok(items);
        });

        app.MapPost("/api/quizQuestions/v1", async (IMongoDatabase db, QuizQuestion model, HttpContext context) =>
        {
            //if (IsValid(model)) Console.WriteLine("quizQ is valid");
            var errors = validationErrors(model);
            if (errors.Count > 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { Errors = errors });
            }

            var collection = db.GetCollection<QuizQuestion>("your_collection_name"); //should be in repository
            model.id = ObjectId.GenerateNewId(DateTime.UtcNow);
            var filter = Builders<QuizQuestion>.Filter.Eq(q => q.id, model.id);
            var update = Builders<QuizQuestion>.Update
                .Set(q => q.question, model.question)
                .Set(q => q.choiceNum, model.choiceNum)
                .SetOnInsert(q => q.id, model.id); // This ensures that the _id field is set if it's a new document.

   
            var options = new UpdateOptions
            {
                IsUpsert = true // Set this option to perform an upsert.
            };

            await collection.UpdateOneAsync(filter, update, options);
            return Results.Ok(model);
        });


        app.Run();
    }

    static bool IsValid(object obj)
    {
        var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(obj, validationContext, validationResults, true);
    }

    static List<string> validationErrors(object obj)
    {
        var errors = new List<string>();
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);
        foreach (var validationResult in validationResults)
        {
            errors.Add(validationResult.ErrorMessage);
        }
        return errors;
    }


    public static string getQuiz()
    {
        return "What is what";
    }

}
public class MongoConfiguration
{
    public string connectionString { get; set; }
    public string databaseName { get; set; }
}
public class QuizQuestion
{
    public ObjectId id { get; set; }

    [Required(ErrorMessage = "question is required.")]
    public string question { get; set; }

    [Range(2, int.MaxValue, ErrorMessage = "the number of choices must be greater than 1")]
    public int choiceNum { get; set; }
}

public class Validaiton 
{
    
}