using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class MentorRepository : RepositoryBase<Mentor, MentorModel>, IMentorRepository
{
    private readonly DatabaseContext _context;

    public MentorRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    protected override DbSet<MentorModel> DbSet => _context.Mentors;

    public IAsyncEnumerable<Mentor> QueryAsync(MentorQuery query, CancellationToken cancellationToken)
    {
        IQueryable<MentorModel> queryable = _context.Mentors;

        if (query.UserIds is not [])
        {
            queryable = queryable.Where(x => query.UserIds.Contains(x.UserId));
        }

        if (query.SubjectCourseIds is not [])
        {
            queryable = queryable.Where(x => query.SubjectCourseIds.Contains(x.SubjectCourseId));
        }

        return queryable.AsAsyncEnumerable().Select(MentorMapper.MapTo);
    }

    protected override MentorModel MapFrom(Mentor entity)
    {
        return MentorMapper.MapFrom(entity);
    }

    protected override bool Equal(Mentor entity, MentorModel model)
    {
        return entity.UserId.Equals(model.UserId) && entity.SubjectCourseId.Equals(model.SubjectCourseId);
    }

    protected override void UpdateModel(MentorModel model, Mentor entity) { }
}