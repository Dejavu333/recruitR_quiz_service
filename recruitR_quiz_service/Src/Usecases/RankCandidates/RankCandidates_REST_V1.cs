using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Service;
using recruitR_quiz_service.Usecases.RetrieveQuizToAttend;

namespace recruitR_quiz_service.Usecases.RankCandidates;

[ApiController]
public class RankCandidates_REST_V1 : ControllerBase
{
      //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IRetrieveCandidateService _retrieveCandidateService;
    private readonly ILoggerService _logger;

    public record Result
    {
        public List<CandidateDTO> candidateRanking { get; set; }
    }

    public record Request
    {
        [Required]
        public string? quizInstanceId { get; set; }
    }

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public RankCandidates_REST_V1(IRetrieveCandidateService retrieveCandidateService, ILoggerService logger)
    {
        _retrieveCandidateService = retrieveCandidateService;
        _logger = logger;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpGet("/[controller]")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult handle([FromQuery] Request req)
    {
        _logger?.Debug("retrieving ranking");
        
        var candidateRanking = _retrieveCandidateService    //todo the whole candidate object is not needed, 2 fields (score,email) would be sufficient
            .retrieve(candidateInDb => candidateInDb.quizInstanceId == req.quizInstanceId)
            .OrderByDescending(candidate => candidate.score).ToList();
        
        if (candidateRanking.Count == 0) return NotFound();
        else return Ok(new Result(){candidateRanking = candidateRanking});
    }
}