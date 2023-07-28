using MongoDB.Bson.Serialization.Attributes;

namespace recruitR_quiz_service;
public class QuizInstanceDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId]
    string? quizName { get; set; }

    [LaterThanNowBy(days: 0, hours: 1)]
    DateTime? expirationDate;

    [HasMoreElementsThan(0)]
    List<(string, bool)>? allowedEmail_alreadySolved_pairs;
}