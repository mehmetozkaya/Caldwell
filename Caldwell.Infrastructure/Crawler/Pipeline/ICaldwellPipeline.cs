using Caldwell.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Pipeline
{
    public interface ICaldwellPipeline
    {
        Task Run(Catalog entity);
    }
}
