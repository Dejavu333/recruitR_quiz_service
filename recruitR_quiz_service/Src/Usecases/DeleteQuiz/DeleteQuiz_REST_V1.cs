using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.DeleteQuiz;

public class DeleteQuiz_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IQuizRepository _quizRepository;
    private readonly ILoggerService _logger;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public DeleteQuiz_REST_V1(IQuizRepository quizRepository, ILoggerService logger)
    {
        _quizRepository = quizRepository;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpPost("[controller]")]
    public async Task<ActionResult> handle([FromQuery] string name)
    {
        var deleteResult = await _quizRepository.DeleteOneQuiz(name);
        if (deleteResult.DeletedCount == 0) return NotFound(Msg.NOT_FOUND);
        return Ok(Msg.DELETED);
    }
}
