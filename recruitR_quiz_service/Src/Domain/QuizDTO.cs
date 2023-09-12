using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service;

public class QuizDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId] 
    public string? id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string? title { get; set; }

    [EmailAddress] 
    public string? ownerEmail { get; set; }

    [Range(60, double.MaxValue)]
    public int? overallTimeLimit { get; set; } //todo alg so can't be exploited on the client's side

    [HasMoreElementsThan(0)] 
    public List<QuizQuestionDTO>? quizQuestions { get; set; }

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public QuizDTO(string? id, string? title, string? ownerEmail, int? overallTimeLimit, List<QuizQuestionDTO>? quizQuestions)
    {
        this.id ??= ObjectId.GenerateNewId().ToString();
        this.title = title;
        this.ownerEmail = ownerEmail;
        this.overallTimeLimit = overallTimeLimit;
        this.quizQuestions = quizQuestions;
    }
}