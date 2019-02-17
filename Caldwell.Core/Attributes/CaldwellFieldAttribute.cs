using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Core.Attributes
{
    public class CaldwellFieldAttribute : Attribute
    {
        public string Expression { get; set; }
        public SelectorType SelectorType { get; set; }
    }

    public enum SelectorType
    {
        XPath,
        CssSelector
    }
}
