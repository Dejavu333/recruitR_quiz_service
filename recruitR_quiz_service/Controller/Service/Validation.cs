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
        if (value is IList<object> list && list.Count > this.moreThan) return true;
        else return false;
    }
}

class GreaterThanAttribute : ValidationAttribute
{
    public DateTime date { get; set; }

    public GreaterThanAttribute(double days, double hours)
    {
        this.date = DateTime.Now;
        if (days != null) this.date = this.date.AddDays((double)days);
        if (hours != null) this.date = this.date.AddHours((double)hours);
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
