using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;
using Itmo.Dev.Asap.Core.Users;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class UserServiceMapper
{
    public static partial FindUserByUniversityId.Query MapTo(this FindByUniversityIdRequest request);

    public static partial FindUserById.Query MapTo(this FindByIdRequest request);

    public static partial UpdateUserUniversityId.Command MapTo(this UpdateUniversityIdRequest request);

    public static partial UpdateUserName.Command MapTo(this UpdateNameRequest request);

    public static partial QueryUsers.Query MapTo(this QueryRequest request);

    public static partial FindByUniversityIdResponse MapFrom(this FindUserByUniversityId.Response response);

    public static partial FindByIdResponse MapFrom(this FindUserById.Response response);

    public static partial UpdateUniversityIdResponse MapFrom(this UpdateUserUniversityId.Response response);

    public static partial UpdateNameResponse MapFrom(this UpdateUserName.Response response);

    public static partial QueryResponse MapFrom(this QueryUsers.Response response);

    private static QueryUsers.PageToken MapToPageToken(string value)
        => JsonConvert.DeserializeObject<QueryUsers.PageToken>(value);

    private static string MapToString(QueryUsers.PageToken pageToken)
        => JsonConvert.SerializeObject(pageToken);
}