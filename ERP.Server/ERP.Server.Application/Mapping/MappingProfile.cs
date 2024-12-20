using AutoMapper;
using ERP.Server.Application.Features.Users.UpdateUser;
using ERP.Server.Domain.Entities;

namespace ERP.Server.Application.Mapping;
internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateUserCommand, User>();
    }
}
