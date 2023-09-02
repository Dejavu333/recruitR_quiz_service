using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;
namespace recruitR_quiz_service;

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
        var quizzes = _quizRepository.ReadQuizzes();
        if (quizzes.Count == 0) return NotFound();
        else return Ok(quizzes);
    }

    [HttpGet("/api/v1/GetOneQuiz")]
    [ProducesResponseType(typeof(QuizDTO),StatusCodes.Status200OK)] [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetOneQuiz([FromQuery] string name)
    {
        var quiz = _quizRepository.ReadOneQuiz(quizInDb=>quizInDb.name==name);
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
        return Ok(Msg.DELETED);
    }
}