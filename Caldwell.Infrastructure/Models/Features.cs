using Caldwell.Core.Repository;
using System;
using System.Collections.Generic;

namespace Caldwell.Infrastructure.Models
{
    public partial class Features : IEntity
    {
        public Features()
        {
            FeatureItem = new HashSet<FeatureItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public int CatalogItemId { get; set; }

        public virtual Catalog CatalogItem { get; set; }
        public virtual ICollection<FeatureItem> FeatureItem { get; set; }
    }
}
