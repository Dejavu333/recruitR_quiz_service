using MongoDB.Driver;
using recruitR_quiz_service.Model.Repository;

namespace recruitR_quiz_service.quiz.Repository;

public sealed class MongoQuizRepository : IQuizRepository
{    
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    readonly IMongoDatabase db;
    readonly IMongoCollection<QuizDTO> coll;
    const string QUIZES_COLL_NAME = "quizes";

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public MongoQuizRepository(IMongoDatabase db)
    {
        this.db = db;
        coll = this.db.GetCollection<QuizDTO>(QUIZES_COLL_NAME);
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    public List<QuizDTO> GetAllQuizes()
    {
        //return coll.FindAsync(FilterDefinition<QuizDTO>.Empty).GetAwaiter().GetResult().ToList();
        return coll.AsQueryable().ToList();
    }

    public QuizDTO? GetOneQuiz(QuizDTO quizToGet)
    {
        //return coll.FindAsync(q => q.id == quiz.id).GetAwaiter().GetResult().FirstOrDefault();
        return coll.AsQueryable().Where(q => q.id == quizToGet.id).FirstOrDefault();
    }

    public async Task<ReplaceOneResult> UpsertOneQuiz(QuizDTO quizToUpsert)
    {
        return await coll.ReplaceOneAsync(
            filter: q => q.id == quizToUpsert.id,
            replacement: quizToUpsert,
            options: new ReplaceOptions { IsUpsert = true });
    }

    public DeleteResult DeleteOneQuiz(QuizDTO quizToDelete)
    {
        return coll.DeleteOneAsync(q => q.id == quizToDelete.id).GetAwaiter().GetResult();  
    }
}