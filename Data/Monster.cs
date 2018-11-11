using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class Monster
    {
        public Monster()
        {
            NearbyMonster = new HashSet<NearbyMonster>();
        }

        public int MonsterId { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Accuracy { get; set; }
        public int AttackRate { get; set; }

        public ICollection<NearbyMonster> NearbyMonster { get; set; }
    }
}
