using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Repositories.Students;
using Itmo.Dev.Asap.Core.DataAccess.Repositories.SubjectCourses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Core.DataAccess.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection collection,
        Action<DbContextOptionsBuilder> action)
    {
        return collection.AddDataAccess((_, b) => action.Invoke(b));
    }

    public static IServiceCollection AddDataAccess(
        this IServiceCollection collection,
        Action<IServiceProvider, DbContextOptionsBuilder> action)
    {
        collection.AddDbContext<DatabaseContext>((provider, builder) =>
        {
            action.Invoke(provider, builder);
            builder.ConfigureWarnings(x => x.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
        });

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IStudentRepository, StudentRepository>();
        collection.AddScoped<IMentorRepository, MentorRepository>();
        collection.AddScoped<IAssignmentRepository, AssignmentRepository>();
        collection.AddScoped<IGroupAssignmentRepository, GroupAssignmentRepository>();
        collection.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
        collection.AddScoped<ISubjectRepository, SubjectRepository>();
        collection.AddScoped<ISubjectCourseRepository, SubjectCourseRepository>();
        collection.AddScoped<ISubmissionRepository, SubmissionRepository>();
        collection.AddScoped<IUserAssociationRepository, UserAssociationRepository>();
        collection.AddScoped<IStudentAssignmentRepository, StudentAssignmentRepository>();

        return collection;
    }

    public static Task UseDatabaseContext(this IServiceScope scope)
    {
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        return context.Database.MigrateAsync();
    }
}