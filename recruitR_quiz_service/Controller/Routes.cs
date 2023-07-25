using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using recruitR_quiz_service.Model.Repository;

namespace recruitR_quiz_service;
public partial class Program
{
    public static void mapRoutes(WebApplication app)
    {
        //app.MapGet("/", () => { return "hey"; });

        app.MapGet("/api/v1/GetAllQuizes", (IQuizRepository quizRepository) =>
        {
            var quizes = quizRepository.GetAllQuizes();
            return Results.Ok(quizes);
        });

        app.MapPost("/api/v1/GetOneQuiz", ([FromBody] QuizDTO model, IQuizRepository quizRepository) =>
        {
            var quiz = quizRepository.GetOneQuiz(model);
            return Results.Ok(quiz);
        });

        app.MapPost("/api/v1/UpsertOneQuiz", ([FromBody] QuizDTO model, IQuizRepository quizRepository) =>
        {
            var errors = validationErrors(model);
            foreach (var question in model.quizQuestions)
            {
                var questionErrors = validationErrors(question);
                errors.AddRange(questionErrors); 
            }
            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors });

            var upsertedId = quizRepository.UpsertOneQuiz(model);
            return Results.Ok((ReplaceOneResult.Acknowledged)upsertedId.Result);
        });

        app.MapDelete("/api/v1/DeleteOneQuiz", ([FromBody] QuizDTO model, IQuizRepository quizRepository) =>
        {
            var errors = validationErrors(model);
            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors});

            var deleteResult = quizRepository.DeleteOneQuiz(model);
            return Results.Ok(deleteResult);
        });
    }
}