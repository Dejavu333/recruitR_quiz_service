using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace recruitR_quiz_service.Model.Repository
{
    public interface IQuizRepository
    {
        public QuizDTO GetOneQuiz(QuizDTO quiz);
        public List<QuizDTO> GetAllQuizes();
        public Task<ReplaceOneResult> UpsertOneQuiz(QuizDTO quiz);
        public DeleteResult DeleteOneQuiz(QuizDTO quiz);
    }
}
