using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Core.Attributes
{
    public class CaldwellFieldAttribute : Attribute
    {
        public string XPath { get; set; }
    }
}
