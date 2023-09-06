using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;

public class DeployQuizAndRetrieveQuizAccessTokens_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    // private readonly IReadQuizInstanceService _readQuizInstanceService;
    private readonly IUpsertQuizInstanceService _upsertQuizInstanceService;
    private readonly IGenerateQuizAccessTokensService _generateQuizAccessTokensService;
    private readonly ILoggerService _logger;

    public record Request
    {
        public string quizId;
        public string[] emails;
        public DateTime expirationDate;
    }
    
    public record Result
    {
        public List<(string, string)> emailˇtokenPairs;
        public Result(List<(string, string)> emailˇtokenPairs)
        {
            this.emailˇtokenPairs = emailˇtokenPairs;
        }
    }
    
    
    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public DeployQuizAndRetrieveQuizAccessTokens_REST_V1(
        IUpsertQuizInstanceService upsertQuizInstanceService,
        IGenerateQuizAccessTokensService generateQuizAccessTokensService,
        ILoggerService logger)
    {
        _upsertQuizInstanceService = upsertQuizInstanceService;
        _generateQuizAccessTokensService = generateQuizAccessTokensService;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    public ActionResult<Result> handle([FromBody] Request req)
    {
        var quizInstance = new QuizInstanceDTO(req.quizId, req.expirationDate);
        var emailtokenpairs = new List<(string, string)>();
        foreach (string email in req.emails)
        {
            var c = new CandidateDTO(quizInstance, email);
            quizInstance.addCandidate(c);
            emailtokenpairs.Add( (c.email, c.quizAccessToken) );
        }

        _upsertQuizInstanceService.upsert(quizInstance);

        //transactional
        return Ok(new Result(emailtokenpairs));
    }
}


public interface IGenerateQuizAccessTokensService
{
    public string generate();
}


public interface IUpsertQuizInstanceService
{
    public void upsert(QuizInstanceDTO q);
}


class UpsertQuizInstanceService : IUpsertQuizInstanceService
{
    public void upsert(QuizInstanceDTO q)
    {
        throw new NotImplementedException();
    }
}