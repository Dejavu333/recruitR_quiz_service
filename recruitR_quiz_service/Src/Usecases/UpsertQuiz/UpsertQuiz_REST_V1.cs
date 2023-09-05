using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.UpsertQuiz;

public class UpsertQuiz_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IQuizRepository _quizRepository;
    private readonly ILoggerService _logger;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public UpsertQuiz_REST_V1(IQuizRepository quizRepository, ILoggerService logger)
    {
        _quizRepository = quizRepository;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpPost("[controller]")]
    public async Task<ActionResult> handle([FromBody] QuizDTO quizToUpsert)
    {
        var replaceOneResult = await _quizRepository.UpsertQuiz(quizToUpsert);
        bool isInserted = replaceOneResult.UpsertedId != null;
        bool isModified = replaceOneResult.ModifiedCount > 0;
        if (isInserted) return Ok(Msg.INSERTED);
        else if (isModified) return Ok(Msg.UPDATED);
        else return NotFound();
    }
}
