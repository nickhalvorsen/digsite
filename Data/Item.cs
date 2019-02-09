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
        public int ItemSlotId { get; set; }
        public int Cooldown { get; set; }

        public ItemCategory ItemCategory { get; set; }
        public ItemSlot ItemSlot { get; set; }
        public ICollection<PlayerItem> PlayerItem { get; set; }
    }
}
