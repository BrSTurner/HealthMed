namespace Med.SharedKernel.UoW
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChanges();
    }
}
