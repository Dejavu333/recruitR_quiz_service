using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;

[ApiController]
public class DeployQuizAndRetrieveQuizAccessTokens_REST_V1 : ControllerBase
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IUpsertQuizInstanceService _upsertQuizInstanceService;
    private readonly IBatchUpsertCandidatesService _batchUpsertCandidatesService;
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
        IBatchUpsertCandidatesService batchUpsertCandidatesService,
        ILoggerService logger)
    {
        _upsertQuizInstanceService = upsertQuizInstanceService;
        _batchUpsertCandidatesService = batchUpsertCandidatesService;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpPost("/[controller]")]
    [ProducesResponseType(typeof(Result),StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> handle([FromBody] Request req)
    {   //TODO should check if quiz with req.quizid exists or should use nesting
        var quizInstance = new QuizInstanceDTO(req.quizId, req.expirationDate);
        try
        {
            var res = await _upsertQuizInstanceService.upsert(quizInstance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        var emailTokenPairs = new Dictionary<string, string>();
        var candidatesToUpsert = new List<CandidateDTO>();
        foreach (string email in req.emails)
        {
            var c = new CandidateDTO(quizInstance.id, email);
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
            try
            {
                var r = await _batchUpsertCandidatesService.upsert(candidatesToUpsert);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        //TODO should be transactional
        return Ok(emailTokenPairs);
    }
}

public interface IBatchUpsertCandidatesService
{
    public Task<BulkWriteResult<CandidateDTO>> upsert(List<CandidateDTO> candidatesToUpsert);
}

public class BatchUpsertCandidatesService : IBatchUpsertCandidatesService
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly  ILoggerService _logger;
    private IMongoDatabase  _dbContext;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public BatchUpsertCandidatesService(ILoggerService logger, IMongoDatabase dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    public async Task<BulkWriteResult<CandidateDTO>> upsert(List<CandidateDTO> candidatesToUpsert)
    {
        _logger.Info("upserting candidates");
        var coll = _dbContext.GetCollection<CandidateDTO>("CANDIDATES");

        var bulkWriteModels = candidatesToUpsert.Select(candidate =>
            new ReplaceOneModel<CandidateDTO>(
                Builders<CandidateDTO>.Filter.Eq("quizAccessToken", candidate.quizAccessToken),
                candidate)
            {
                IsUpsert = true
            }
        ).ToList();

        BulkWriteOptions bulkWriteOptions = new BulkWriteOptions { IsOrdered = false };

        BulkWriteResult<CandidateDTO> result = await coll.BulkWriteAsync(bulkWriteModels, bulkWriteOptions);    //InsertMany() wouldn't update

        return result;
    }
}

public interface IUpsertQuizInstanceService
{
    public Task<ReplaceOneResult> upsert(QuizInstanceDTO q);
}

public class UpsertQuizInstanceService : IUpsertQuizInstanceService
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly  ILoggerService _logger;
    private IMongoDatabase  _dbContext;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public UpsertQuizInstanceService(ILoggerService logger, IMongoDatabase dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    public async Task<ReplaceOneResult> upsert(QuizInstanceDTO q)
    {
        _logger.Info("upserting quiz instance");
        var coll = _dbContext.GetCollection<QuizInstanceDTO>("QUIZINSTANCES");
        var result = await coll.ReplaceOneAsync(
            filter: quizInstanceInDb => quizInstanceInDb.id == q.id,
            replacement: q,
            options: new ReplaceOptions { IsUpsert = true });

        return result;        
    }
}

