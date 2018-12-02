using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class PlayerItem
    {
        public int PlayerItemId { get; set; }
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public byte IsEquipped { get; set; }
        public int CurrentCooldown { get; set; }

        public Item Item { get; set; }
        public Player Player { get; set; }
    }
}
