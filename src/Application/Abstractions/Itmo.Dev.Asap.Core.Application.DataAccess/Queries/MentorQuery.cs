using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record MentorQuery(Guid[] UserIds, Guid[] SubjectCourseIds);