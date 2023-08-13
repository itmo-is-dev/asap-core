using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class SubjectRepository : RepositoryBase<Subject, SubjectModel>, ISubjectRepository
{
    private readonly DatabaseContext _context;

    public SubjectRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    protected override DbSet<SubjectModel> DbSet => _context.Subjects;

    public IAsyncEnumerable<Subject> QueryAsync(SubjectQuery query, CancellationToken cancellationToken)
    {
        IQueryable<SubjectModel> queryable = DbSet;

        if (query.Ids is not [])
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.Id));
        }

        if (query.Names is not [])
        {
            queryable = queryable.Where(x => query.Names.Contains(x.Title));
        }

        if (query.MentorIds is not [])
        {
            queryable = queryable.Where(subject =>
                subject.SubjectCourses.SelectMany(sc => sc.Mentors)
                    .Select(m => m.UserId)
                    .Any(x => query.MentorIds.Contains(x)));
        }

        return queryable.AsAsyncEnumerable().Select(SubjectMapper.MapTo);
    }

    protected override SubjectModel MapFrom(Subject entity)
    {
        return SubjectMapper.MapFrom(entity);
    }

    protected override bool Equal(Subject entity, SubjectModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(SubjectModel model, Subject entity)
    {
        model.Title = entity.Title;
    }
}