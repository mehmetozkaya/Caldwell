using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Models;
using Caldwell.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Pipeline
{
    public class CaldwellPipeline<TEntity> : ICaldwellPipeline<TEntity> where TEntity : class, IEntity
    {
        private readonly IGenericRepository<TEntity> _repository;

        public CaldwellPipeline()
        {
            _repository = new GenericRepository<TEntity>();
        }

        public async Task Run(IEnumerable<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                await _repository.CreateAsync(entity);
            }
        }
    }
}
