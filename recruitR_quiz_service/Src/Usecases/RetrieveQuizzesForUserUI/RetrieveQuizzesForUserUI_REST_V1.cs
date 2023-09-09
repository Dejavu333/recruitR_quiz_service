using recruitR_quiz_service.Service;
using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Repository;

namespace recruitR_quiz_service.Usecases.RetreiveQuizzesForUserUI;

[ApiController]
public class RetrieveQuizzesForUserUI_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IQuizRepository _quizRepository;
    private readonly ILoggerService _logger;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public RetrieveQuizzesForUserUI_REST_V1(IQuizRepository quizRepository, ILoggerService logger)
    {
        _quizRepository = quizRepository;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpGet("/[controller]")]
    [ProducesResponseType(typeof(List<QuizDTO>),StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<QuizDTO>> handle([FromQuery]string? name)
    {
        _logger?.Debug("some debug msg");
        
        if (name is null)
        {
            var quizzes = _quizRepository.ReadQuizzes();
            if (quizzes.Count == 0) return NotFound();
            else return Ok(quizzes);
        }
        else 
        {
            var quiz = _quizRepository.ReadQuiz(quizInDb=>quizInDb.title==name);
            if (quiz is null) return NotFound();
            else return Ok(new List<QuizDTO>{quiz});
        }
    }
}