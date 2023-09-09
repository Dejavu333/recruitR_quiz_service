using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.RetrieveQuizToAttend;

[ApiController]
public class RetrieveQuizzesToAttend_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IQuizRepository _quizRepository;
    private readonly ILoggerService _logger;
    private readonly IRetrieveCandidateService _retrieveCandidateService;
    private readonly IRetrieveQuizInstanceService _retrieveQuizInstanceService;

    public class Request
    {
        [Required]
        public string? quizAccessToken { get; set; }
    }

    public class Result
    {
        public QuizDTO quiz { get; set; }
    }

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public RetrieveQuizzesToAttend_REST_V1(
        ILoggerService logger,
        IQuizRepository quizRepository,
        IRetrieveCandidateService retrieveCandidateService,
        IRetrieveQuizInstanceService retrieveQuizInstanceService)
    {
        _quizRepository = quizRepository;
        _retrieveCandidateService = retrieveCandidateService;
        _retrieveQuizInstanceService = retrieveQuizInstanceService;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpGet("/[controller]")]
    [ProducesResponseType(typeof(Result),StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult handle([FromQuery]Request req)
    {
        _logger?.Debug("retrieving quiz to attend...");

        var emailAndQuizInstanceId = CandidateDTO.getEmailAndQuizInstanceIdFromQuizAccessToken(req.quizAccessToken);
        var email = emailAndQuizInstanceId.email;
        var quizInstanceId = emailAndQuizInstanceId.quizInstanceId;
        var candidate = _retrieveCandidateService.retrieve(candidateInDb => candidateInDb.email == email && candidateInDb.quizInstanceId == quizInstanceId);
        if (candidate is null || email == "invalid" || quizInstanceId == "invalid")
        {
            return Ok("invalid quiz access token");
        }

        if (candidate.didAttendQuiz == true)
        {
            return Ok("already attended quiz");
        }

        var quizinstance = _retrieveQuizInstanceService.retrieve(quizInstanceInDb => quizInstanceInDb.id == candidate.quizInstanceId);
        if (quizinstance.expirationDate < DateTime.UtcNow)
        {
            return Ok($"quiz has expired at {quizinstance.expirationDate}");
        }
        
        var quiz = _quizRepository.ReadQuiz(quizInDb=>quizInDb.id==quizinstance.quizId);
        if (quiz is null) return NotFound();
        else return Ok(new Result(){quiz = quiz});
    }
}

public interface IRetrieveQuizInstanceService
{
    public QuizInstanceDTO? retrieve(Expression<Func<QuizInstanceDTO,bool>> filter);
}

public class RetrieveQuizInstanceService_Mongo : IRetrieveQuizInstanceService
{
    // public IMongoDatabase dbContext;

    public QuizInstanceDTO? retrieve(Expression<Func<QuizInstanceDTO, bool>> filter)
    {
        return new QuizInstanceDTO("asdid", DateTime.UtcNow);
    }
}


public interface IRetrieveCandidateService
{
    public CandidateDTO? retrieve(Expression<Func<CandidateDTO,bool>> filter);
}

public class RetrieveCandidateService_Mongo : IRetrieveCandidateService
{
    // public IMongoDatabase dbContext;

    public CandidateDTO? retrieve(Expression<Func<CandidateDTO, bool>> filter)
    {
        // return new CandidateDTO("string", "string@example.com", true, 10);
        return new CandidateDTO("string", "string@example.com", false, 10);
    }
}