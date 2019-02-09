using Caldwell.Core.Repository;
using System;
using System.Collections.Generic;

namespace Caldwell.Infrastructure.Models
{
    public partial class CompareItem : IEntity
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string CompareDesc { get; set; }
        public int CatalogItemId { get; set; }
        public int? CompareId { get; set; }

        public virtual Compares Compare { get; set; }
    }
}
