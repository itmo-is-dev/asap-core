using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record StudentGroupQuery(
    Guid[] Ids,
    Guid[] StudentIds,
    string[] NamePatterns,
    Guid? Cursor,
    int? Limit);