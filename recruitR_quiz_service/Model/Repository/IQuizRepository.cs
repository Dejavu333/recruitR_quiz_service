using System.Linq.Expressions;
using MongoDB.Driver;

namespace recruitR_quiz_service;

public interface IQuizRepository
{
    public List<QuizDTO> ReadQuizzes();
    public QuizDTO? ReadOneQuiz(Expression<Func<QuizDTO,bool>> filter);
    public Task<ReplaceOneResult> UpsertOneQuiz(QuizDTO quizToUpsert);
    public Task<DeleteResult> DeleteOneQuiz(string targetName);
}
