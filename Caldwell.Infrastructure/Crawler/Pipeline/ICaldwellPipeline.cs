using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Pipeline
{
    public interface ICaldwellPipeline<TEntity> where TEntity : class, IEntity
    {
        Task Run(IEnumerable<TEntity> entity);
    }
}
