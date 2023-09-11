using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace recruitR_quiz_service;

public class CandidateDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    // [BsonId] [Required] public string? id { get; set; }

    [Required] 
    public string? quizInstanceId { get; set; }

    [EmailAddress] [Required] 
    public string? email { get; set; }

    public bool didAttendQuiz { get; set; }

    public double score { get; set; }
    
    [BsonId]
    public string quizAccessToken { get; set; }    //becomes _id column in mongodb

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public CandidateDTO(string? quizInstanceId, string? email, bool didAttendQuiz = false, double score = 0)
    {
        // this.id ??= ObjectId.GenerateNewId().ToString();
        this.quizInstanceId = quizInstanceId;
        this.email = email;
        this.didAttendQuiz = didAttendQuiz;
        this.score = score;
        this.quizAccessToken = this.generateQuizAccessToken(this.email, this.quizInstanceId);
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    /// <summary>
    /// Generates a quiz access token for the candidate from the candidate's email and quiz instance id. Uses base64 encoding.
    /// </summary>
    /// <returns> The quiz access token as a string. </returns>
    private string generateQuizAccessToken(string email, string quizInstanceId)
    {
        string emailAndQuizInstanceId = email + " " + quizInstanceId;
        byte[] emailAndQuizInstanceIdBytes = System.Text.Encoding.UTF8.GetBytes(emailAndQuizInstanceId);
        string emailAndQuizInstanceIdBase64 = Convert.ToBase64String(emailAndQuizInstanceIdBytes);
        return emailAndQuizInstanceIdBase64;
    }

    /// <summary>
    /// Gets the email and quiz instance id from the quiz access token. Uses base64 decoding.
    /// </summary>
    /// <returns> The email and quiz instance id as a (email, quizInstanceId) tuple. </returns>
    public static (string email, string quizInstanceId) getEmailAndQuizInstanceIdFromQuizAccessToken(
        string quizAccessToken)
    {
        try
        {
            byte[] quizAccessTokenBytes = Convert.FromBase64String(quizAccessToken);
            string quizAccessTokenString = System.Text.Encoding.UTF8.GetString(quizAccessTokenBytes);
            string email = quizAccessTokenString.Split(" ")[0];
            string quizInstanceId = quizAccessTokenString.Split(" ")[1];
            return (email: email, quizInstanceId: quizInstanceId);
        }
        catch (Exception e)
        {
            return (email: "invalid", quizInstanceId: "invalid");
        }
    }
}