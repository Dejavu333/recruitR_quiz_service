using MongoDB.Bson;

namespace recruitR_quiz_service;
public class QuizInstanceDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    ObjectId quizId { get; set; } = ObjectId.Empty;

    [LaterThanNowBy(days: 0, hours: 1, ErrorMessage = "expirationDate must be greater than current date")]
    DateTime expirationDate;

    [HasMoreElementsThan(1, ErrorMessage = "quizAnswers must have at least 2 elements")]
    List<(string, bool)> allowedEmail_alreadySolved_pairs;
}