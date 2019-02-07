using Caldwell.Core.Repository;
using System;
using System.Collections.Generic;

namespace Caldwell.Infrastructure.Models
{
    public partial class Wishlists : IEntity
    {
        public Wishlists()
        {
            WishlistItem = new HashSet<WishlistItem>();
        }

        public int Id { get; set; }
        public string OwnerId { get; set; }

        public virtual ICollection<WishlistItem> WishlistItem { get; set; }
    }
}
