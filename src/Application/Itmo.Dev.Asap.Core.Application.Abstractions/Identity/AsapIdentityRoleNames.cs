namespace Itmo.Dev.Asap.Core.Application.Abstractions.Identity;

public static class AsapIdentityRoleNames
{
    public const string AdminRoleName = "admin";
    public const string ModeratorRoleName = "moderator";
    public const string MentorRoleName = "mentor";

    public const string AtLeastMentor = $"{AdminRoleName}, {ModeratorRoleName}, {MentorRoleName}";
}