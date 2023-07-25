using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service;
public class QuizQuestionDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [Required]
    public string? question { get; set; }

    [HasMoreElementsThan(1, ErrorMessage = "choices must have at least 2 elements")]
    public List<string>? choices { get; set; }
}