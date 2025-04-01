using FluentValidation.Results;

namespace Med.SharedKernel.Models
{
    public abstract class Input
    {
        public abstract ValidationResult Validate(); 
    }
}
