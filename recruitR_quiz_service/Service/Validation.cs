using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service;
class HasMoreElementsThanAttribute : ValidationAttribute
{
    public int moreThan { get; set; }
    public HasMoreElementsThanAttribute(int moreThan)
    {
        this.moreThan = moreThan;
    }
    public override bool IsValid(object? value)
    {
        if (value is ICollection coll && coll.Count > this.moreThan) return true;
        else return false;
    }
}

class LaterThanNowByAttribute : ValidationAttribute
{
    public DateTime date { get; set; }

    public LaterThanNowByAttribute(double days, double hours)
    {
        this.date = DateTime.Now;
        this.date = this.date.AddDays(days);
        this.date = this.date.AddHours(hours);
    }

    public override bool IsValid(object? value)
    {
        if (value is DateTime dateToCheck && dateToCheck > this.date) return true;
        else return false;
    }
}

public static partial class Program
{
    static bool IsValid(object obj)
    {
        var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(obj, validationContext, validationResults, true);
    }

    static List<string> validationErrors(object obj)
    {
        var errors = new List<string>();
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);
        foreach (var validationResult in validationResults)
        {
            errors.Add(validationResult.ErrorMessage);
        }
        return errors;
    }
}
