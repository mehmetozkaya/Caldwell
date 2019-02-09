using Caldwell.Core.Repository;
using System;
using System.Collections.Generic;

namespace Caldwell.Infrastructure.Models
{
    public partial class Compares : IEntity
    {
        public Compares()
        {
            CompareItem = new HashSet<CompareItem>();
        }

        public int Id { get; set; }
        public string BuyerId { get; set; }

        public virtual ICollection<CompareItem> CompareItem { get; set; }
    }
}
