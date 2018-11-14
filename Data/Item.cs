using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class Item
    {
        public Item()
        {
            PlayerItem = new HashSet<PlayerItem>();
        }

        public int ItemId { get; set; }
        public int ItemCategoryId { get; set; }
        public string Name { get; set; }

        public ItemCategory ItemCategory { get; set; }
        public ICollection<PlayerItem> PlayerItem { get; set; }
    }
}
