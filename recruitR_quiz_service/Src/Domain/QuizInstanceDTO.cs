using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service;

public class QuizInstanceDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [Required] 
    public string? quizId { get; set; }

    [BsonId] [Required] 
    public string? id { get; set; }
    
    [LaterThanNowBy(days: 0, hours: 1)] 
    public DateTime? expirationDate {get;set;}
    
    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public QuizInstanceDTO(string quizId, DateTime expirationDate)
    {
        this.id ??= ObjectId.GenerateNewId().ToString();
        this.quizId = quizId;
        this.expirationDate = expirationDate;
    }
    
    //---------------------------------------------
    // methods
    //---------------------------------------------

}
