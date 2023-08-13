using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;

internal class AddFractionDeadlinePolicy
{
    public record Command(Guid SubjectCourseId, TimeSpan SpanBeforeActivation, double Fraction) : IRequest;
}