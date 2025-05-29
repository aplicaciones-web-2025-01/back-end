namespace learning_center_back.Shared.Domain;

public interface IUnitOfWork
{
    Task CompleteAsync();
}