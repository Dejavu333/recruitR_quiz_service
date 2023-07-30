using MongoDB.Driver;

namespace recruitR_quiz_service;

static class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        addServices(builder);

        WebApplication app = builder.Build();
        useServices(app);
        app.Run();
    }

    static void addServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IQuizRepository>((serviceProvider) =>
        {
            return new MongoQuizRepository(new MongoConfiguration(serviceProvider.GetRequiredService<IConfiguration>()));
        });
    }

    static void useServices(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseHttpsRedirection(); // dotnet dev-certs https --trust
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}

//TODO global error handling