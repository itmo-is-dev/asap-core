using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class AssignmentRepository : RepositoryBase<Assignment, AssignmentModel>, IAssignmentRepository
{
    private readonly DatabaseContext _context;

    public AssignmentRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    protected override DbSet<AssignmentModel> DbSet => _context.Assignments;

    public IAsyncEnumerable<Assignment> QueryAsync(AssignmentQuery query, CancellationToken cancellationToken)
    {
        IQueryable<AssignmentModel> queryable = DbSet;

        if (query.Ids.Count is not 0)
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.Id));
        }

        if (query.SubjectCourseIds.Count is not 0)
        {
            queryable = queryable.Where(x => query.SubjectCourseIds.Contains(x.SubjectCourseId));
        }

        if (query.OrderByOrder is not null)
        {
            queryable = query.OrderByOrder.Value switch
            {
                OrderDirection.Ascending => queryable.OrderBy(x => x.Order),
                OrderDirection.Descending => queryable.OrderByDescending(x => x.Order),
                _ => queryable,
            };
        }

        return queryable.AsAsyncEnumerable().Select(AssignmentMapper.MapTo);
    }

    protected override AssignmentModel MapFrom(Assignment entity)
    {
        throw new UnreachableException($"Method {nameof(AssignmentRepository)}.{nameof(MapFrom)} should be called");
    }

    protected override bool Equal(Assignment entity, AssignmentModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(AssignmentModel model, Assignment entity)
    {
        model.Title = entity.Title;
        model.ShortName = entity.ShortName;
        model.Order = entity.Order;
        model.MinPoints = entity.MinPoints.Value;
        model.MaxPoints = entity.MaxPoints.Value;
    }
}