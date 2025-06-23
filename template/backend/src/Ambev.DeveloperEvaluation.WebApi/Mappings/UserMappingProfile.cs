using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserRequest, CreateUserCommand>();
            CreateMap<CreateUserResult, CreateUserResponse>();
            CreateMap<GetUserResult, GetUserResponse>();

            CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));

            CreateMap<Guid, DeleteUserCommand>()
            .ConstructUsing(id => new DeleteUserCommand(id));
        }
    }
}
