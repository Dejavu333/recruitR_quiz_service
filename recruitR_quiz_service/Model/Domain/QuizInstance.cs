using MongoDB.Bson;

namespace recruitR_quiz_service;
public class QuizInstance
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    ObjectId quizId;

    [GreaterThan(days:0, hours:1, ErrorMessage="expirationDate must be greater than current date by at least 1 hour")]
    DateTime expirationDate;

    [HasMoreElementsThan(0, ErrorMessage = "quizAnswers must have at least one element")]
    List<(string, bool)>? allowedEmail_alreadySolved_pairs;
}