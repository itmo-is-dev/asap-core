using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class UserController : UserService.UserServiceBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<FindByUniversityIdResponse> FindByUniversityId(
        FindByUniversityIdRequest request,
        ServerCallContext context)
    {
        FindUserByUniversityId.Query query = request.MapTo();
        FindUserByUniversityId.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<FindByIdResponse> FindById(FindByIdRequest request, ServerCallContext context)
    {
        FindUserById.Query query = request.MapTo();
        FindUserById.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateUniversityIdResponse> UpdateUniversityId(
        UpdateUniversityIdRequest request,
        ServerCallContext context)
    {
        UpdateUserUniversityId.Command command = request.MapTo();
        UpdateUserUniversityId.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateNameResponse> UpdateName(UpdateNameRequest request, ServerCallContext context)
    {
        UpdateUserName.Command command = request.MapTo();
        UpdateUserName.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<QueryResponse> Query(QueryRequest request, ServerCallContext context)
    {
        QueryUsers.Query query = request.MapTo();
        QueryUsers.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }
}