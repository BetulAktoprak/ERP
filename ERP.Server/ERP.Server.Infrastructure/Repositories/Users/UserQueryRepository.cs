using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.Users;

internal sealed class UserQueryRepository : QueryRepository<User>, IUserQueryRepository
{
    private readonly ApplicationDbContext context;
    public UserQueryRepository(ApplicationDbContext context) : base("users")
    {
        this.context = context;
    }

    public async Task<User?> GetUserByConfirmEmailCodeAsync(Guid code, CancellationToken cancellationToken)
    {
        User? user = await context.Users.FirstOrDefaultAsync(p => p.MailConfirmCode == code, cancellationToken);

        return user;
    }

    public async Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken)
    {
        FilterDefinition<User> isDeletedFilter = Builders<User>.Filter.Eq(p => p.IsDeleted, false);
        FilterDefinition<User> userNameOrEmailFilter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq(p => p.UserName, userNameOrEmail),
            Builders<User>.Filter.Eq(p => p.Email, userNameOrEmail)
        );
        FilterDefinition<User> isActiveFilter = Builders<User>.Filter.Eq(p => p.IsActive, true);

        FilterDefinition<User> combinedFilter = Builders<User>.Filter.And(isDeletedFilter, userNameOrEmailFilter, isActiveFilter);
        return await _collection.Find(combinedFilter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Email", email);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Email", email);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsUserNameExistsAsync(string userName, CancellationToken cancellationToken)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("UserName", userName);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}