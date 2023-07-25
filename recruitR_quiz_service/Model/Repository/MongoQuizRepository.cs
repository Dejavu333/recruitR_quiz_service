using Microsoft.AspNetCore.Mvc.Diagnostics;
using MongoDB.Driver;
using recruitR_quiz_service.Model.Repository;

namespace recruitR_quiz_service.quiz.Repository;

public sealed class MongoQuizRepository : IQuizRepository
{    
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    readonly IMongoDatabase db;
    IMongoCollection<QuizDTO> coll;
    const string QUIZES_COLL_NAME = "quizes";

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public MongoQuizRepository(IMongoDatabase db)
    {
        this.db = db;
        refreshCollection();
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    void refreshCollection() {
        this.coll = this.db.GetCollection<QuizDTO>(QUIZES_COLL_NAME);
    }

    public List<QuizDTO> GetAllQuizes()
    {
        //return coll.FindAsync(FilterDefinition<QuizDTO>.Empty).GetAwaiter().GetResult().ToList();
        return coll.AsQueryable().ToList();
    }

    public QuizDTO? GetOneQuiz(string targetName)
    {
        //return coll.FindAsync(q => q.name == quiz.name).GetAwaiter().GetResult().FirstOrDefault();
        return coll.AsQueryable().Where(q => q.name == targetName).FirstOrDefault();
    }

    public async Task<ReplaceOneResult> UpsertOneQuiz(QuizDTO quizToUpsert)
    {
        var result = await coll.ReplaceOneAsync(
            filter: q => q.name == quizToUpsert.name,
            replacement: quizToUpsert,
            options: new ReplaceOptions { IsUpsert = true });

        refreshCollection();
        return result;
    }

    public async Task<DeleteResult> DeleteOneQuiz(string targetName)
    {
        var result = await coll.DeleteOneAsync(q => q.name == targetName);

        refreshCollection();
        return result;
    }
}