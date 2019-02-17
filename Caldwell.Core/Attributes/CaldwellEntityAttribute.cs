using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Core.Attributes
{
    public class CaldwellEntityAttribute : Attribute
    {
        public string XPath { get; set; }
    }
}
