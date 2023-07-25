using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Model.Repository;

namespace recruitR_quiz_service;
public partial class Program
{
    public static void mapRoutes(WebApplication app)
    {
        app.MapGet("/api/v1/GetAllQuizes", (IQuizRepository quizRepository) =>
        {
            //get
            var quizes = quizRepository.GetAllQuizes();
            //response
            return Results.Ok(quizes);
        });

        app.MapGet("/api/v1/GetOneQuiz", ([FromQuery] string name, IQuizRepository quizRepository) =>
        {
            //get
            var quiz = quizRepository.GetOneQuiz(name);
            //response
            return Results.Ok(quiz);
        });

        app.MapPost("/api/v1/UpsertOneQuiz", async ([FromBody] QuizDTO quizToUpsert, IQuizRepository quizRepository) =>
        {
            //valiation
            var errors = validationErrors(quizToUpsert);
            foreach (var q in quizToUpsert.quizQuestions)
            {
                var quizQuestionErrors = validationErrors(q);
                errors.AddRange(quizQuestionErrors);
            }
            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors });
            //upsert
            var replaceOneResult = await quizRepository.UpsertOneQuiz(quizToUpsert);
            //response
            return Results.Ok(replaceOneResult.UpsertedId);
        });

        app.MapDelete("/api/v1/DeleteOneQuiz", async ([FromQuery] string name, IQuizRepository quizRepository) =>
        {
            //valiation
            var errors = validationErrors(name);
            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors });
            //delete
            var deleteResult = await quizRepository.DeleteOneQuiz(name);
            //response
            return Results.Ok(deleteResult.DeletedCount);
        });
    }
}