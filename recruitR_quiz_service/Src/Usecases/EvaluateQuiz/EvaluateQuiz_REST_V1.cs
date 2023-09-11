using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;
using recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;

namespace recruitR_quiz_service.Usecases.EvaluateQuiz;

[ApiController]
public class EvaluateQuiz_REST_V1 : ControllerBase 
{
      //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IQuizRepository _quizRepository;
    private readonly ILoggerService _logger;
    private readonly IBatchUpsertCandidatesService _batchUpsertCandidatesService;

    public record Result
    {
        public bool isOperationSuccess { get; set; }
    }

    public record Request
    {
        [Required]
        public string? quizId { get; set; }
        [Required]
        public List<QuestionAnswerPair> questionAnswerPairs { get; set; }    //based on the indecies of the quizquestions and its answerind
        [Required]
        public int timeSpent { get; set; }
        [Required]
        public string? quizAccessToken { get; set; }

        public record QuestionAnswerPair
        {
            public int questionIndex { get; set; }
            public int answerIndex { get; set; }
        }
    }
    

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public EvaluateQuiz_REST_V1(IQuizRepository quizRepository, ILoggerService logger, IBatchUpsertCandidatesService batchUpsertCandidatesService)
    {
        _quizRepository = quizRepository;
        _logger = logger;
        _batchUpsertCandidatesService = batchUpsertCandidatesService;
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    [HttpPost("/[controller]")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult handle([FromBody] Request req)
    {
        var quiz = _quizRepository.ReadQuiz(quizInDb => quizInDb.id == req.quizId);
        if (quiz is null) return NotFound();
        double score = 0;
        const double SCALE = 10;
        foreach (var qaPair in req.questionAnswerPairs)
        {
            int questionInd = qaPair.questionIndex;
            int candidateAnswserInd = qaPair.answerIndex;
            if (quiz.quizQuestions[questionInd].answerInd == candidateAnswserInd) score++;
        }
        double timeLeft = (double)(quiz.overallTimeLimit) - req.timeSpent;
        score *= SCALE;
        double offset = mapValue(timeLeft, 0, (double)quiz.overallTimeLimit, 0, 1*SCALE);
        _logger.Debug("offset" + offset);
        _logger.Info("offset" + offset);
        score += offset;

        var token = CandidateDTO.getEmailAndQuizInstanceIdFromQuizAccessToken(req.quizAccessToken);
        var candidateToUpdate = new CandidateDTO(token.quizInstanceId, token.email, true, score);
        var r = _batchUpsertCandidatesService.upsert(new List<CandidateDTO>(){candidateToUpdate});
        return Ok(new Result() { isOperationSuccess = r.IsCompleted });
        
        double mapValue(double input, double inputMin, double inputMax, double outputMin, double outputMax)
        {
            if (input <= inputMin)
            {
                return Math.Round(outputMin, 4);
            }
            else if (input >= inputMax)
            {
                return Math.Round(outputMax, 4);
            }
            else
            {
                // interpolation
                double result = outputMin + (outputMax - outputMin) * ((input - inputMin) / (inputMax - inputMin));
                return Math.Round(result, 4);
            }
        }
    }
}