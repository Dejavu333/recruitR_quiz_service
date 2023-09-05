using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service;

public class QuizDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId] [Required] 
    public string? id { get; set; }
    
    [Required] [StringLength(150, MinimumLength = 3)]
    public string? title { get; set; }

    [HasMoreElementsThan(0)] 
    public List<QuizQuestionDTO>? quizQuestions { get; set; }
}