using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service;
public class QuizQuestionDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [Required]
    public string? question { get; set; }

    [HasMoreElementsThan(1)]
    public List<string>? choices { get; set; }
}