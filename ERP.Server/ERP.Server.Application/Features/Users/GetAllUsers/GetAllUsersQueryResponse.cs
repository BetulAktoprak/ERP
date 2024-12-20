namespace ERP.Server.Application.Features.Users.GetAllUsers;

public sealed record GetAllUsersQueryResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string UserName,
    string Email,
    bool IsEmailConfirmed,
    bool IsActive,
    DateTime CreateAt,
    DateTime? UpdateAt
    );
