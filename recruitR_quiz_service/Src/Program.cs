using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using System.Text;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;
using recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;
using recruitR_quiz_service.Usecases.RetrieveQuizToAttend;


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

        builder.Services.AddOpenTelemetry()    // tracing
            .ConfigureResource(resource => resource.AddService(serviceName: builder.Environment.ApplicationName))
            .WithTracing((tracing) =>
            {   
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddSource("Microsoft.AspNetCore.Hosting");
                var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"]; 
                if (tracingOtlpEndpoint != null)
                {
                    tracing.AddZipkinExporter(zipkinOptions => { zipkinOptions.Endpoint = new Uri(tracingOtlpEndpoint); });    // docker run -d -p 9411:9411 --name zipkin openzipkin/zipkin
                }
                // else
                // {
                //     tracing.AddConsoleExporter();
                // }
            });
        builder.Services.AddControllers();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddSwaggerGen(options => {options.CustomSchemaIds(type => $"{type.Name}_{System.Guid.NewGuid()}");});    // openapi specs
        // DI
        builder.Services.AddSingleton<IQuizRepository>((serviceProvider) =>
        {
            return new MongoQuizRepository(new MongoConfiguration(serviceProvider.GetRequiredService<IConfiguration>()));
        });
        builder.Services.AddSingleton<ILoggerService, RabbitMQLogger>();
        builder.Services.AddSingleton<IUpsertQuizInstanceService, UpsertQuizInstanceService>();
        builder.Services.AddSingleton<IRetrieveCandidateService, RetrieveCandidateService_Mongo>();
        builder.Services.AddSingleton<IRetrieveQuizInstanceService, RetrieveQuizInstanceService_Mongo>();
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