using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories.Students;

public class StudentRepository : RepositoryBase<Student, StudentModel>, IStudentRepository
{
    private readonly DatabaseContext _context;
    private readonly IStudentEventVisitor _visitor;

    public StudentRepository(DatabaseContext context) : base(context)
    {
        _context = context;
        _visitor = new StudentRepositoryEventVisitor(context);
    }

    protected override DbSet<StudentModel> DbSet => _context.Students;

    public IAsyncEnumerable<Student> QueryAsync(StudentQuery query, CancellationToken cancellationToken)
    {
        IQueryable<StudentModel> queryable = DbSet;

        if (query.Ids is not [])
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.UserId));
        }

        if (query.GroupIds is not [])
        {
            queryable = queryable
                .Where(x => query.GroupIds.Contains(x.StudentGroupId!.Value));
        }

        if (query.AssignmentIds is not [])
        {
            queryable = queryable
                .Where(student => student.StudentGroup!.SubjectCourseGroups
                    .SelectMany(x => x.SubjectCourse.Assignments)
                    .Select(x => x.Id)
                    .Any(x => query.AssignmentIds.Contains(x)));
        }

        if (query.SubjectCourseIds is not [])
        {
            queryable = queryable
                .Where(student => student.StudentGroup!.SubjectCourseGroups
                    .Any(x => query.SubjectCourseIds.Contains(x.SubjectCourse.Id)));
        }

        if (query.GroupNamePatterns is not [])
        {
            queryable = queryable.Where(x => query.GroupNamePatterns.Any(pattern =>
                EF.Functions.ILike(x.StudentGroup!.Name, pattern)));
        }

        if (query.UniversityIds is not [])
        {
            queryable = queryable
                .Select(x => new
                {
                    student = x,
                    association = x.User.Associations.OfType<IsuUserAssociationModel>().SingleOrDefault(),
                })
                .Where(x => query.UniversityIds.Contains(x.association!.UniversityId))
                .Select(x => x.student);
        }

        if (query.FullNamePatterns is not [])
        {
            string[] patterns = query.FullNamePatterns.Select(pattern => $"%{pattern}%").ToArray();

            queryable = queryable.Where(student =>
                patterns.Any(pattern => EF.Functions.ILike(student.User.FirstName, pattern))
                || patterns.Any(pattern => EF.Functions.ILike(student.User.MiddleName, pattern))
                || patterns.Any(pattern => EF.Functions.ILike(student.User.LastName, pattern)));
        }

        queryable = queryable.OrderBy(x => x.UserId);

        if (query.Cursor is not null)
        {
            queryable = queryable.Where(x => x.UserId > query.Cursor);
        }

        if (query.Limit is not null)
        {
            queryable = queryable.Take(query.Limit.Value);
        }

        queryable = queryable
            .Include(x => x.User)
            .ThenInclude(x => x.Associations)
            .Include(x => x.StudentGroup);

        return queryable.AsAsyncEnumerable().Select(StudentMapper.MapTo);
    }

    public ValueTask ApplyAsync(IStudentEvent evt, CancellationToken cancellationToken)
    {
        return evt.AcceptAsync(_visitor, cancellationToken);
    }

    protected override StudentModel MapFrom(Student entity)
    {
        return StudentMapper.MapFrom(entity);
    }

    protected override bool Equal(Student entity, StudentModel model)
    {
        return entity.UserId.Equals(model.UserId);
    }

    protected override void UpdateModel(StudentModel model, Student entity)
    {
        model.StudentGroupId = entity.Group?.Id;
    }
}