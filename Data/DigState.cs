using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class DigState
    {
        public DigState()
        {
            NearbyMonster = new HashSet<NearbyMonster>();
        }

        public int DigStateId { get; set; }
        public int PlayerId { get; set; }
        public int Depth { get; set; }
        public decimal Fuel { get; set; }
        public byte IsPaused { get; set; }

        public Player Player { get; set; }
        public ICollection<NearbyMonster> NearbyMonster { get; set; }
    }
}
