using MongoDB.Bson.Serialization.Attributes;

namespace recruitR_quiz_service;
public class QuizInstanceDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId]
    string quizName { get; set; }

    [LaterThanNowBy(days: 0, hours: 1, ErrorMessage = $"expirationDate must be greater than current date by at least 1hour")]
    DateTime? expirationDate;

    [HasMoreElementsThan(1, ErrorMessage = "quizAnswers must have at least 2 elements")]
    List<(string, bool)>? allowedEmail_alreadySolved_pairs;
}