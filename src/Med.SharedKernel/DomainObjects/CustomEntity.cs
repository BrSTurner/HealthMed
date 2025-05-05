using FluentValidation.Results;

namespace Med.SharedKernel.DomainObjects
{
    public abstract class CustomEntity<EntityId>
    {
        public EntityId? Id { get; set; }
        public DateTime CreatedAt { get; set; }

        protected CustomEntity()
        {
            CreatedAt = DateTime.Now;
        }

        public virtual ValidationResult Validate()
            => throw new NotImplementedException();
    }
}
