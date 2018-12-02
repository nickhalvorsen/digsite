using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class NearbyMonster
    {
        public int NearbyMonsterId { get; set; }
        public int MonsterId { get; set; }
        public int CurrentHealth { get; set; }
        public int DigStateId { get; set; }
        public int CurrentAttackCooldown { get; set; }

        public DigState DigState { get; set; }
        public Monster Monster { get; set; }
    }
}
