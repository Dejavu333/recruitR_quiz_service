using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service;
public class QuizDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    public ObjectId? id { get; set; } = null;

    [Required]
    public string name { get; set; }

    [HasMoreElementsThan(0)]
    public List<QuizQuestionDTO> quizQuestions { get; set; }
}