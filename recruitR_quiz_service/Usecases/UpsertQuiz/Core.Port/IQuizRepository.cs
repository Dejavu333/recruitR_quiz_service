using System.Linq.Expressions;
using MongoDB.Driver;

namespace recruitR_quiz_service.Repository;

public interface IQuizRepository
{
    public List<QuizDTO> ReadQuizzes();
    public QuizDTO? ReadQuiz(Expression<Func<QuizDTO,bool>> filter);
    public Task<ReplaceOneResult> UpsertQuiz(QuizDTO quizToUpsert);
    public Task<DeleteResult> DeleteQuiz(string targetName);
}
