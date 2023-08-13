using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands.AddFractionDeadlinePolicy;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class AddFractionDeadlinePolicyHandler : IRequestHandler<Command>
{
    private readonly IPersistenceContext _context;

    public AddFractionDeadlinePolicyHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        var penalty = new FractionDeadlinePenalty(request.SpanBeforeActivation, request.Fraction);
        DeadlinePenaltyAddedEvent evt = subjectCourse.AddDeadlinePenalty(penalty);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}