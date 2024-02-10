using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.RetrieveQuizzesForUserUI;

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
        public string? owneremail { get; set; }
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
        _logger?.Info("retrieving quizzes for user UI");

        if (req.title is null && req.id is null && req.owneremail is null)
        {
            // Case 1: All properties are null, retrieve all quizzes
            var quizzes = _quizRepository.ReadQuizzes();
            if (quizzes.Count == 0) return NotFound();
            else return Ok(quizzes);
        }
        else
        {
            // Initialize filters to build the query based on non-null conditions
            var filters = new List<Expression<Func<QuizDTO, bool>>>();

            if (!string.IsNullOrWhiteSpace(req.title))
            {
                // Case 2: Title is not null, search by title
                filters.Add(quizInDb => quizInDb.title == req.title);
            }

            if (req.id != null)
            {
                // Case 3: ID is not null, search by ID
                filters.Add(quizInDb => quizInDb.id == req.id);
            }

            if (!string.IsNullOrWhiteSpace(req.owneremail))
            {
                // Case 4: Email is not null, search by owneremail (assuming owneremail property exists in Quiz)
                filters.Add(quizInDb => quizInDb.ownerEmail == req.owneremail);
            }

            // Combine the filters using AND logic
            var combinedFilter = filters.Aggregate(
                (filter1, filter2) => filter1.And(filter2)
            );

            var quiz = _quizRepository.ReadQuiz(combinedFilter);

            if (quiz is null) return NotFound();
            else return Ok(new List<QuizDTO> { quiz });
        }
    }
}

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var invokedExpr = Expression.Invoke(right, left.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var invokedExpr = Expression.Invoke(right, left.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, invokedExpr), left.Parameters);
    }
}
