using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace recruitR_quiz_service;

public class CandidateDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [BsonId] [Required] 
    public string? id { get; set; }
    
    // [BsonId] [Required]
    // public string? quizInstanceId { get; set; }
    public QuizInstanceDTO refToQuizInstance {get;set;}
    
    [EmailAddress] [Required]
    public string? email { get; set;}

    public bool didAttendQuiz { get; set; }

    public double score {get;set;}
    
    public string quizAccessToken { get; set; }

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public CandidateDTO(string? id, QuizInstanceDTO refToQuizInstance, string? email, bool didAttendQuiz = false, double score = 0)
    {
        this.id = id;
        // this.quizInstanceId = quizInstanceId;
        this.refToQuizInstance = refToQuizInstance;
        this.email = email;
        this.didAttendQuiz = didAttendQuiz;
        this.score = score;
        this.quizAccessToken = generateQuizAccessToken(this.email, this.refToQuizInstance.id);
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    /// <summary>
    /// Generates a quiz access token for the candidate from the candidate's email and quiz instance id. Uses base64 encoding.
    /// </summary>
    /// <returns> The quiz access token as a string. </returns>
    public static string generateQuizAccessToken(string email, string quizInstanceId) 
    {
        //encode email and quizInstanceId into a string in base64 and return it
        string emailAndQuizInstanceId = email +" "+ quizInstanceId;
        byte[] emailAndQuizInstanceIdBytes = System.Text.Encoding.UTF8.GetBytes(emailAndQuizInstanceId);
        string emailAndQuizInstanceIdBase64 = Convert.ToBase64String(emailAndQuizInstanceIdBytes);
        return emailAndQuizInstanceIdBase64;
    }

    /// <summary>
    /// Gets the email and quiz instance id from the quiz access token. Uses base64 decoding.
    /// </summary>
    /// <returns> The email and quiz instance id as a (email, quizInstanceId) tuple. </returns>
    public static (string,string) getEmailAndQuizInstanceIdFromQuizAccessToken(string quizAccessToken)
    {
        //decode quizAccessToken from base64 and return it
        byte[] quizAccessTokenBytes = Convert.FromBase64String(quizAccessToken);
        string quizAccessTokenString = System.Text.Encoding.UTF8.GetString(quizAccessTokenBytes);
        string email = quizAccessTokenString.Split(" ")[0];
        string quizInstanceId = quizAccessTokenString.Split(" ")[1];
        return (email, quizInstanceId);
    }
}
