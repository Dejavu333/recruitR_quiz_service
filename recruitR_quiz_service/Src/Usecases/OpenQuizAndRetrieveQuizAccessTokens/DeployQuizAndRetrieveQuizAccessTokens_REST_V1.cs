using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;

[ApiController]
public class DeployQuizAndRetrieveQuizAccessTokens_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    // private readonly IReadQuizInstanceService _readQuizInstanceService;
    private readonly IUpsertQuizInstanceService _upsertQuizInstanceService;
    // private readonly IGenerateQuizAccessTokensService _generateQuizAccessTokensService;
    private readonly ILoggerService _logger;

    public new class Request
    {
        public string quizId { get; set; }
        public string[] emails { get; set; }
        public DateTime expirationDate { get; set; }
    }
    
    public class Result
    {
        public Dictionary<string, string> emailTokenPairs { get; set;}

        public Result(Dictionary<string, string> emailTokenPairs)
        {
            this.emailTokenPairs = emailTokenPairs;
        }
    }
    
    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public DeployQuizAndRetrieveQuizAccessTokens_REST_V1(
        IUpsertQuizInstanceService upsertQuizInstanceService,
        // IGenerateQuizAccessTokensService generateQuizAccessTokensService,
        ILoggerService logger)
    {
        _upsertQuizInstanceService = upsertQuizInstanceService;
        // _generateQuizAccessTokensService = generateQuizAccessTokensService;
        _logger = logger;
        //todo if logger is null -> fallbacklogger
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpPost("/[controller]")]
    [ProducesResponseType(typeof(Result),StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Result> handle([FromBody] Request req)
    {
        var quizInstance = new QuizInstanceDTO(req.quizId, req.expirationDate);
        var quizInstanceId = _upsertQuizInstanceService.upsert(quizInstance);
        
        var emailTokenPairs = new Dictionary<string, string>();
        var candidatesToUpsert = new List<CandidateDTO>();
        foreach (string email in req.emails)
        {
            var c = new CandidateDTO(quizInstanceId, email);
            if (c.email is null || emailTokenPairs.ContainsKey(c.email))
            { 
                continue;
            }
            else
            {
                emailTokenPairs.Add(c.email, c.quizAccessToken);
                candidatesToUpsert.Add(c);
            }
        }
        
        if (candidatesToUpsert.Count > 0)
        {
            _logger.Info("upserting candidates in batch");
            // var collection = _mongoDatabase.GetCollection<CandidateDTO>("Candidates");
            // collection.InsertMany(candidatesToUpsert);
        }
        
        //transactional
        return Ok(emailTokenPairs);
    }
}

public interface IUpsertQuizInstanceService
{
    public string upsert(QuizInstanceDTO q);
}

public class UpsertQuizInstanceService : IUpsertQuizInstanceService
{
    private readonly  ILoggerService _logger;

    public UpsertQuizInstanceService(ILoggerService logger)
    {
        _logger = logger;
    }

    public string upsert(QuizInstanceDTO q)
    {
        _logger.Info("upserted quizinstance");
        return "someid";
    }
}

