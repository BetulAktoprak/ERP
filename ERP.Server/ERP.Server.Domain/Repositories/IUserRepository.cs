using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;

public interface IUserCommandRepository : ICommandRepository<User>
{
}
public interface IUserQueryRepository : IQueryRepository<User>
{
    Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsUserNameExistsAsync(string userName, CancellationToken cancellationToken);
    Task<User?> GetUserByConfirmEmailCodeAsync(Guid code, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}
