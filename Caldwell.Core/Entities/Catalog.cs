using Caldwell.Core.Attributes;
using Caldwell.Core.Repository;
using System;
using System.Collections.Generic;

namespace Caldwell.Infrastructure.Models
{
    //[CaldwellEntity(XPath = "//*[@id='ozet']")]
    [CaldwellEntity(XPath = "//*[@id='LeftSummaryPanel']/div[1]")]
    public partial class Catalog : IEntity
    {
        public Catalog()
        {
            Features = new HashSet<Features>();
        }

        public int Id { get; set; }

        //[CaldwellField(Expression = "//*[@id='ozet']/div[1]/div/h1/a(text)", SelectorType =SelectorType.XPath)]
        [CaldwellField(Expression = "//*[@id='itemTitle']/text()", SelectorType =SelectorType.XPath)]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public double Star { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
        public string AlfonsoPoint { get; set; }
        public string VersusPoint { get; set; }
        public string AntutuPoint { get; set; }
        public string Battery { get; set; }
        public string Camera { get; set; }
        public string Screen { get; set; }
        public string Storage { get; set; }
        public string Ram { get; set; }
        public string Cpu { get; set; }

        public virtual CatalogBrand CatalogBrand { get; set; }
        public virtual CatalogType CatalogType { get; set; }
        public virtual ICollection<Features> Features { get; set; }
    }
}
