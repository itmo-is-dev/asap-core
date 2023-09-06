using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class StudentGroupRepository : IStudentGroupRepository
{
    private readonly DatabaseContext _context;

    public StudentGroupRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<StudentGroup> QueryAsync(StudentGroupQuery query, CancellationToken cancellationToken)
    {
        IQueryable<StudentGroupModel> queryable = _context.StudentGroups;

        if (query.Ids is not [])
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.Id));
        }

        if (query.StudentIds is not [])
        {
            queryable = queryable.Where(g => g.Students
                .Select(x => x.UserId)
                .Any(x => query.StudentIds.Contains(x)));
        }

        if (query.NamePatterns is not [])
        {
            string[] namePatterns = query.NamePatterns.Select(pattern => '%' + pattern + '%').ToArray();

            queryable = queryable.Where(x => namePatterns.Any(p =>
                EF.Functions.ILike(x.Name, p)));
        }

        if (query.ExcludedSubjectCourseIds is not [])
        {
            queryable = queryable.Where(
                group => !group.SubjectCourseGroups.Any(
                    sc => query.ExcludedSubjectCourseIds.Contains(sc.SubjectCourseId)));
        }

        if (query.Cursor is not null)
        {
            queryable = queryable
                .OrderBy(x => x.Id)
                .Where(x => x.Id > query.Cursor);
        }

        if (query.Limit is not null)
        {
            queryable = queryable.Take(query.Limit.Value);
        }

        var finalQueryable = queryable.Select(studentGroup => new
        {
            studentGroup,
            students = studentGroup.Students.Select(x => x.UserId),
        });

        return finalQueryable.AsAsyncEnumerable().Select(x => MapTo(x.studentGroup, x.students));
    }

    public void Update(StudentGroup studentGroup)
    {
        EntityEntry<StudentGroupModel> entry = GetEntry(
            studentGroup.Id,
            () => StudentGroupMapper.MapFrom(studentGroup));

        StudentGroupModel model = entry.Entity;
        model.Name = studentGroup.Name;

        entry.State = EntityState.Modified;
    }

    public void Add(StudentGroup studentGroup)
    {
        StudentGroupModel model = StudentGroupMapper.MapFrom(studentGroup);
        _context.StudentGroups.Add(model);
    }

    private static StudentGroup MapTo(StudentGroupModel model, IEnumerable<Guid> studentIds)
    {
        return StudentGroupMapper.MapTo(model, studentIds.ToHashSet());
    }

    private EntityEntry<StudentGroupModel> GetEntry(Guid studentGroupId, Func<StudentGroupModel> modelFactory)
    {
        StudentGroupModel? existing = _context.StudentGroups.Local
            .FirstOrDefault(model => model.Id.Equals(studentGroupId));

        return existing is not null
            ? _context.Entry(existing)
            : _context.StudentGroups.Attach(modelFactory.Invoke());
    }
}