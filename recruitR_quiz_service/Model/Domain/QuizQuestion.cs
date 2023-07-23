using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service;
public class QuizQuestion
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    public ObjectId id { get; set; }

    [Required]
    public string question { get; set; }

    [Range(2, int.MaxValue, ErrorMessage = "the number of choices must be greater than 1")]
    public int choiceCount { get; set; } 

    public List<string> choices {get; set;}

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public QuizQuestion( string question, List<string> choices)
    {
        this.question = question;
        this.choices = choices;
        this.choiceCount = this.choices.Count;
    }
}