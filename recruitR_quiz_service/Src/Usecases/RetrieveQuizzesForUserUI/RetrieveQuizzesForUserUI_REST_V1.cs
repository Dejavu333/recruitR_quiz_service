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

    public record Result
    {
        public List<QuizDTO> quizzes { get; set; }
    }

    public record Request
    {
        public string? id { get; set; }
        public string? title { get; set; }
    }

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
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<QuizDTO>> handle([FromQuery] Request req)
    {
        _logger?.Debug("retrieving quizzes for user UI");

        if (req.title is null && req.id is null)
        {
            var quizzes = _quizRepository.ReadQuizzes();
            if (quizzes.Count == 0) return NotFound();
            else return Ok(quizzes);
        }

        if (req.id is null)
        {
            var quiz = _quizRepository.ReadQuiz(quizInDb => quizInDb.title == req.title);
            if (quiz is null) return NotFound();
            else return Ok(new List<QuizDTO> { quiz });
        }
        else if (req.title is null)
        {
            var quiz = _quizRepository.ReadQuiz(quizInDb => quizInDb.id == req.id);
            if (quiz is null) return NotFound();
            else return Ok(new List<QuizDTO> { quiz });
        }
        else
        {
            var quiz = _quizRepository.ReadQuiz(quizInDb => quizInDb.title == req.title && quizInDb.id == req.id);
            if (quiz is null) return NotFound();
            else return Ok(new List<QuizDTO> { quiz });
        }
    }
}