using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class ItemCategory
    {
        public ItemCategory()
        {
            Item = new HashSet<Item>();
        }

        public int ItemTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
