namespace FamilyCookbook.Exceptions
{
    public class ModelValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ModelValidationException(IDictionary<string, string[]> errors) 
            : base("One or more validation errors occurred!")
        {
            Errors = errors;  
        }
    }
}
