using Microsoft.AspNetCore.Mvc;

namespace recruitR_quiz_service;

//public partial class Program
//{
//    public static void mapRoutes(WebApplication app)
//    {
//        app.MapGet("/api/v1/GetAllQuizes", (IQuizRepository quizRepository) =>
//        {
//            //get
//            var quizes = quizRepository.GetAllQuizes();
//            //response
//            if (quizes.Count == 0) return Results.NotFound();
//            else return Results.Ok(quizes);
//        });

//        app.MapGet("/api/v1/GetOneQuiz", ([FromQuery] string name, IQuizRepository quizRepository) =>
//        {
//            //get
//            var quiz = quizRepository.GetOneQuiz(name);
//            //response
//            if (quiz == null) return Results.NotFound();
//            else return Results.Ok(quiz);
//        });

//        app.MapPost("/api/v1/UpsertOneQuiz", async ([FromBody] QuizDTO quizToUpsert, IQuizRepository quizRepository) =>
//        {
//            //validation
//            var errors = recruitR_quiz_service.Program.validationErrors(quizToUpsert);
//            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors });
//            //upsert
//            var replaceOneResult = await quizRepository.UpsertOneQuiz(quizToUpsert);
//            //response
//            bool isInserted = replaceOneResult.UpsertedId != null;
//            bool isModified = replaceOneResult.ModifiedCount > 0;
//            if (isInserted) return Results.Ok(Msg.INSERTED);
//            else if (isModified) return Results.Ok(Msg.UPDATED);
//            else return Results.NotFound();
//        });

//        app.MapDelete("/api/v1/DeleteOneQuiz", async ([FromQuery] string name, IQuizRepository quizRepository) =>
//        {
//            //validation
//            var errors = recruitR_quiz_service.Program.validationErrors(name);
//            if (errors.Count > 0) return Results.BadRequest(new { Errors = errors });
//            //delete
//            var deleteResult = await quizRepository.DeleteOneQuiz(name);
//            //response
//            if (deleteResult.DeletedCount == 0) return Results.NotFound(Msg.NOT_FOUND);
//            return Results.Ok("deleted");
//        });
//    }
//}

[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizRepository _quizRepository;

    public QuizController(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    [HttpGet("/api/v1/GetAllQuizes")]
    public ActionResult<List<QuizDTO>> GetAllQuizes()
    {
        var quizes = _quizRepository.GetAllQuizes();
        if (quizes.Count == 0) return NotFound();
        else return Ok(quizes);
    }

    [HttpGet("/api/v1/GetOneQuiz")]
    public ActionResult<QuizDTO> GetOneQuiz([FromQuery] string name)
    {
        var quiz = _quizRepository.GetOneQuiz(name);
        if (quiz == null) return NotFound();
        else return Ok(quiz);
    }

    [HttpPost("/api/v1/UpsertOneQuiz")]
    public async Task<ActionResult> UpsertOneQuiz([FromBody] QuizDTO quizToUpsert)
    {
        var replaceOneResult = await _quizRepository.UpsertOneQuiz(quizToUpsert);
        bool isInserted = replaceOneResult.UpsertedId != null;
        bool isModified = replaceOneResult.ModifiedCount > 0;
        if (isInserted) return Ok(Msg.INSERTED);
        else if (isModified) return Ok(Msg.UPDATED);
        else return NotFound();
    }

    [HttpDelete("/api/v1/DeleteOneQuiz")]
    public async Task<ActionResult> DeleteOneQuiz([FromQuery] string name)
    {
        var deleteResult = await _quizRepository.DeleteOneQuiz(name);
        if (deleteResult.DeletedCount == 0) return NotFound(Msg.NOT_FOUND);
        return Ok("deleted");
    }
}
