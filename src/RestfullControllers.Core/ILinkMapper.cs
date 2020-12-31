using System.Collections.Generic;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Core
{
    public interface ILinkMapper<TEntity>
    {
        IEnumerable<Link> MapControllerLinks();
        IEnumerable<Link> MapEntityLinks(TEntity entity);
        IEnumerable<Link> MapEntityLinks(IEnumerable<TEntity> entities);
        IEnumerable<Link> MapSubEntityLinks(object entity);
    }
}