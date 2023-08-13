using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.DataAccess.Models;

public partial class SubmissionModel : IEntity<Guid>
{
    public SubmissionModel(
        Guid id,
        int code,
        string payload,
        double? rating,
        double? extraPoints,
        SpbDateTime submissionDate,
        Guid studentId,
        Guid studentGroupId,
        Guid assignmentId,
        SubmissionStateKind state)
        : this(id)
    {
        Code = code;
        Payload = payload;
        Rating = rating;
        ExtraPoints = extraPoints;
        SubmissionDate = submissionDate;
        StudentId = studentId;
        StudentGroupId = studentGroupId;
        AssignmentId = assignmentId;
        State = state;
    }

    public int Code { get; set; }

    public string Payload { get; set; }

    public double? Rating { get; set; }

    public double? ExtraPoints { get; set; }

    public SpbDateTime SubmissionDate { get; set; }

    public Guid StudentId { get; set; }

    public virtual StudentModel Student { get; set; }

    public Guid StudentGroupId { get; set; }

    public Guid AssignmentId { get; set; }

    public virtual GroupAssignmentModel GroupAssignment { get; set; }

    public SubmissionStateKind State { get; set; }
}