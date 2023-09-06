using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service;

public class QuizInstanceDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId] [Required] 
    public string? quizId { get; set; }

    [BsonId] [Required] 
    public string? id { get; set; }

    [HasMoreElementsThan(0)] public List<CandidateDTO> candidates { get; set; }

    [LaterThanNowBy(days: 0, hours: 1)] 
    public DateTime? expirationDate {get;set;}
    
    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public QuizInstanceDTO(string quizId, DateTime expirationDate)
    {
        this.candidates = new();
        this.quizId = quizId;
        this.expirationDate = expirationDate;
    }
    
    //---------------------------------------------
    // methods
    //---------------------------------------------
    public void addCandidate(CandidateDTO candidate)
    {
        this.candidates.Add(candidate);
    }
}
