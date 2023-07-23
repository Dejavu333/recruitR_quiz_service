using MongoDB.Bson;

namespace recruitR_quiz_service;
public class Quiz
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    ObjectId id;

    [HasMoreElementsThan(0)]
    List<QuizQuestion> quizQuestions;
}