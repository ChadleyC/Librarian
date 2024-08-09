using System.ComponentModel.DataAnnotations;

namespace SupportWave.Librarian.Attributes;

public class PastDate : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }
        
        DateTime date = (DateTime)value;
        return date < DateTime.Now;
    }
}