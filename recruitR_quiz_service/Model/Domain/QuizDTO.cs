using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace recruitR_quiz_service;
public class QuizDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId]
    [Required]
    public string? name { get; set; }

    [HasMoreElementsThan(0)]
    public List<QuizQuestionDTO>? quizQuestions { get; set; }
}