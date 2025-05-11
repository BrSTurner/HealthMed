

using FluentValidation.Results;

namespace Med.SharedKernel.Models
{
    public class DomainResult
    {
        public bool IsSuccess { get; set; }
        public dynamic? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static DomainResult Create(bool success, List<string>? errors = null, dynamic? data = null)
            => new() { IsSuccess = success, Data = data, Errors = errors };

        public static DomainResult Success(dynamic? data = null)
            => new() { IsSuccess = true,  Data = data, Errors = [] };
        public static DomainResult Error(ValidationResult result)
            => new() { IsSuccess = false, Errors = result?.Errors?.Select(e => e.ErrorMessage)?.ToList() };
        public static DomainResult Error(string error)
            => new() { IsSuccess = false, Errors = [error] };
    }
}
