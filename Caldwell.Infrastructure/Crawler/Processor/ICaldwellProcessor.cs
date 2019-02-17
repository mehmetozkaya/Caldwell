using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public interface ICaldwellProcessor<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> Process(HtmlDocument document);
    }
}
