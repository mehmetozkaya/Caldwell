using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Models;
using Caldwell.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Pipeline
{
    public class CaldwellPipeline : ICaldwellPipeline
    {
        private readonly IGenericRepository<Catalog> _repository;

        public CaldwellPipeline()
        {
            _repository = new GenericRepository<Catalog>();
        }

        public async Task Run(IEnumerable<Catalog> entityList)
        {
            foreach (var entity in entityList)
            {
                await _repository.CreateAsync(entity);
            }            
        }
    }
}
