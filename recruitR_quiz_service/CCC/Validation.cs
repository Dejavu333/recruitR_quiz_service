using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace recruitR_quiz_service.Service;

class HasMoreElementsThanAttribute : ValidationAttribute
{
    public int moreThan { get; set; }

    public HasMoreElementsThanAttribute(int moreThan)
    {
        this.moreThan = moreThan;
    }
    public override bool IsValid(object? value)
    {
        return value is ICollection coll && coll.Count > this.moreThan;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must have more than {moreThan} elements.";
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
        return value is DateTime dateToCheck && dateToCheck > this.date;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be later than {date}.";
    }
}

//public static partial class Program
//{
//    static bool IsValid(object obj)
//    {
//        var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
//        var validationResults = new List<ValidationResult>();
//        return Validator.TryValidateObject(obj, validationContext, validationResults, true);
//    }

//    static List<string> validationErrors(object obj)
//    {
//        var errors = new List<string>();
//        RecursiveValidateObject(obj, ref errors);
//        return errors;
//    }

//    static void RecursiveValidateObject(object obj, ref List<string> errors)
//    {
//        var validationResults = new List<ValidationResult>();
//        bool isValid = Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);

//        if (!isValid)
//        {
//            foreach (var validationResult in validationResults)
//            {
//                errors.Add(validationResult.ErrorMessage ?? string.Empty);
//            }
//        }
//        var properties = obj.GetType().GetProperties().Where(prop => prop.CanRead
//                                                                     && !prop.PropertyType.IsValueType
//                                                                     && prop.PropertyType != typeof(string));
//        foreach (var property in properties)
//        {
//            var value = property.GetValue(obj);
//            if (value == null)
//            {
//                continue;
//            }

//            if (value is IList list)
//            {
//                foreach (var item in list)
//                {
//                    if (item != null)
//                    {
//                        RecursiveValidateObject(item, ref errors);
//                    }
//                }
//            }
//            else
//            {
//                RecursiveValidateObject(value, ref errors);
//            }
//        }
//    }
//}