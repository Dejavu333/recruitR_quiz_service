using MongoDB.Driver;

namespace recruitR_quiz_service
{
    public interface IQuizRepository
    {
        public List<QuizDTO> GetAllQuizes();
        public QuizDTO? GetOneQuiz(string targetName);
        public Task<ReplaceOneResult> UpsertOneQuiz(QuizDTO quizToUpsert);
        public Task<DeleteResult> DeleteOneQuiz(string targetName);
    }
}
