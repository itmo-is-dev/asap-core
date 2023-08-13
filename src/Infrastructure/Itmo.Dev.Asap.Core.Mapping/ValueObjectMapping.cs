using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.SubmissionStateWorkflows;
using Itmo.Dev.Asap.Core.Domain.Tools;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class ValueObjectMapping
{
    public static DateTime AsDateTime(this DateOnly date)
    {
        return date.ToDateTime(new TimeOnly(0));
    }

    public static DateTime AsDateTime(this SpbDateTime date)
    {
        return date.Value;
    }

    public static DateOnly AsDateOnly(this DateTime date)
    {
        return DateOnly.FromDateTime(date);
    }

    public static SubmissionStateWorkflowTypeDto AsDto(this SubmissionStateWorkflowType type)
    {
        return type switch
        {
            SubmissionStateWorkflowType.ReviewOnly => SubmissionStateWorkflowTypeDto.ReviewOnly,
            SubmissionStateWorkflowType.ReviewWithDefense => SubmissionStateWorkflowTypeDto.ReviewWithDefense,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    public static SubmissionStateDto AsDto(this SubmissionStateKind stateKind)
    {
        return stateKind switch
        {
            SubmissionStateKind.Active => SubmissionStateDto.Active,
            SubmissionStateKind.Inactive => SubmissionStateDto.Inactive,
            SubmissionStateKind.Deleted => SubmissionStateDto.Deleted,
            SubmissionStateKind.Completed => SubmissionStateDto.Completed,
            SubmissionStateKind.Reviewed => SubmissionStateDto.Reviewed,
            SubmissionStateKind.Banned => SubmissionStateDto.Banned,
            _ => throw new ArgumentOutOfRangeException(nameof(stateKind), stateKind, null),
        };
    }

    public static SubmissionStateWorkflowType AsValueObject(this SubmissionStateWorkflowTypeDto dto)
    {
        return dto switch
        {
            SubmissionStateWorkflowTypeDto.ReviewOnly => SubmissionStateWorkflowType.ReviewOnly,
            SubmissionStateWorkflowTypeDto.ReviewWithDefense => SubmissionStateWorkflowType.ReviewWithDefense,
            _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, null),
        };
    }

    public static SpbDateTime AsSpbDateTime(this DateTime date)
    {
        return new SpbDateTime(date);
    }
}