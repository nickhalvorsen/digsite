using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class ItemSlot
    {
        public ItemSlot()
        {
            Item = new HashSet<Item>();
        }

        public int ItemSlotId { get; set; }
        public string Description { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
