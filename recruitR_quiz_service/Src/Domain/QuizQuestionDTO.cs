using System.ComponentModel.DataAnnotations;
using recruitR_quiz_service.Service;

namespace recruitR_quiz_service;

public class QuizQuestionDTO
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    [Required] [StringLength(1000, MinimumLength = 10)]
    public string? question { get; set; }

    [HasMoreElementsThan(1)] 
    public List<string>? choices { get; set; }
}