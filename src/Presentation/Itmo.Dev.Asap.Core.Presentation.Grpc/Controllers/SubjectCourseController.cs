using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubjectCourseController : SubjectCourseService.SubjectCourseServiceBase
{
    private readonly IMediator _mediator;
    private readonly ISubjectCourseUpdateService _subjectCourseUpdateService;

    public SubjectCourseController(IMediator mediator, ISubjectCourseUpdateService subjectCourseUpdateService)
    {
        _mediator = mediator;
        _subjectCourseUpdateService = subjectCourseUpdateService;
    }

    public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        GetSubjectCourseById.Query query = request.MapTo();
        GetSubjectCourseById.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        CreateSubjectCourse.Command command = request.MapTo();
        CreateSubjectCourse.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        UpdateSubjectCourse.Command command = request.MapTo();
        UpdateSubjectCourse.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetStudentsResponse> GetStudents(GetStudentsRequest request, ServerCallContext context)
    {
        GetSubjectCourseStudents.Query query = request.MapTo();
        GetSubjectCourseStudents.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetAssignmentsResponse> GetAssignments(
        GetAssignmentsRequest request,
        ServerCallContext context)
    {
        GetAssignmentsBySubjectCourse.Query query = request.MapTo();
        GetAssignmentsBySubjectCourse.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetGroupsResponse> GetGroups(GetGroupsRequest request, ServerCallContext context)
    {
        GetSubjectCourseGroupsBySubjectCourseId.Query query = request.MapTo();

        GetSubjectCourseGroupsBySubjectCourseId.Response response = await _mediator
            .Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetStudentGroupQueueResponse> GetStudentGroupQueue(
        GetStudentGroupQueueRequest request,
        ServerCallContext context)
    {
        GetSubmissionsQueue.Query query = request.MapTo();
        GetSubmissionsQueue.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<Empty> ForceSyncStudentGroupQueue(
        ForceSyncStudentGroupQueueRequest request,
        ServerCallContext context)
    {
        var notification = new SubjectCourseGroupQueueOutdated.Notification(
            Guid.Parse(request.SubjectCourseId),
            Guid.Parse(request.StudentGroupId));

        await _mediator.Publish(notification, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> AddDeadline(AddDeadlineRequest request, ServerCallContext context)
    {
        AddFractionDeadlinePolicy.Command command = request.MapTo();
        await _mediator.Send(command, context.CancellationToken);

        return new Empty();
    }

    public override Task<Empty> ForceSyncPoints(ForceSyncPointsRequest request, ServerCallContext context)
    {
        _subjectCourseUpdateService.UpdatePoints(Guid.Parse(request.SubjectCourseId));

        return Task.FromResult(new Empty());
    }

    public override async Task<UpdateMentorsResponse> UpdateMentors(
        UpdateMentorsRequest request,
        ServerCallContext context)
    {
        UpdateMentors.Command command = request.MapTo();
        await _mediator.Send(command, context.CancellationToken);

        return new UpdateMentorsResponse();
    }

    public override async Task<GetMentorsResponse> GetMentors(GetMentorsRequest request, ServerCallContext context)
    {
        var query = new GetSubjectCourseMentors.Query(request.SubjectCourseId.ToGuid());
        GetSubjectCourseMentors.Response response = await _mediator.Send(query, context.CancellationToken);

        return new GetMentorsResponse
        {
            MentorIds = { response.MentorIds.Select(x => x.ToString()) },
        };
    }

    public override async Task<QueryResponse> Query(QueryRequest request, ServerCallContext context)
    {
        var query = new QuerySubjectCourses.Query(request.SubjectCourseIds.Select(x => x.ToGuid()));
        QuerySubjectCourses.Response response = await _mediator.Send(query, context.CancellationToken);

        return new QueryResponse
        {
            SubjectCourses = { response.SubjectCourses.Select(x => x.MapToProtoModel()) },
        };
    }
}