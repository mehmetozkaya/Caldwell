using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Request
{
    public class CaldwellRequest : ICaldwellRequest
    {        
        public string Url { get; set; }
        public long TimeOut { get; set; }
    }
}
