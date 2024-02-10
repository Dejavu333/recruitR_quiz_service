using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace recruitR_quiz_service.Repository;

public sealed class MongoConfiguration {
    public string connectionString { get; }
    public string databaseName { get; }

    public MongoConfiguration(IConfiguration appConfig)
    {
        this.connectionString = appConfig.GetSection("MongoDB")["ConnectionString"];
        this.databaseName = appConfig.GetSection("MongoDB")["DatabaseName"];
    }
}

public sealed class MongoQuizRepository : IQuizRepository {
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private readonly IMongoDatabase _db;
    private const string QUIZES_COLL_NAME = "QUIZZES";

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public MongoQuizRepository(MongoConfiguration mongoConfig)
    {
        this._db = new MongoClient(mongoConfig.connectionString).GetDatabase(mongoConfig.databaseName);
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    private IMongoCollection<QuizDTO> _collection()
    {
        return this._db.GetCollection<QuizDTO>(QUIZES_COLL_NAME);
    }

    public List<QuizDTO> ReadQuizzes()
    {
        //return collection().FindAsync(FilterDefinition<QuizDTO>.Empty).GetAwaiter().GetResult().ToList();
        return this._collection().AsQueryable().ToList();
    }

    public QuizDTO? ReadQuiz(Expression<Func<QuizDTO, bool>> filter)
    {
        //return collection().FindAsync(q => q.name == quiz.name).GetAwaiter().GetResult().FirstOrDefault();
        return this._collection().AsQueryable().Where(filter).Single();
    }

    public async Task<ReplaceOneResult> UpsertQuiz(QuizDTO quizToUpsert)
    {
        var result = await this._collection().ReplaceOneAsync(
            filter: q => q.id == quizToUpsert.id,
            replacement: quizToUpsert,
            options: new ReplaceOptions { IsUpsert = true }
        );
        return result;
    }

    public async Task<DeleteResult> DeleteQuiz(string targetName)
    {
        var result = await this._collection().DeleteOneAsync(q => q.title == targetName);
        return result;
    }
}
