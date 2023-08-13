using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record UserQuery(
    Guid[] Ids,
    string[] FullNamePatterns,
    int[] UniversityIds,
    OrderDirection? OrderByLastName,
    int? Cursor,
    int? Limit);