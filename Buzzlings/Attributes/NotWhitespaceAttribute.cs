using System.ComponentModel.DataAnnotations;

namespace Buzzlings.Web.Attributes
{
    public class NotWhitespaceAttribute : ValidationAttribute
    {
        public NotWhitespaceAttribute() : base("The field cannot contain only whitespace.")
        { 

        }

        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return !string.IsNullOrWhiteSpace(str); // Checks for empty or whitespace-only string
            }

            return true; // if not a string, consider it valid (or handle it differently if needed)
        }
    }
}
