using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class Buff
    {
        public Buff()
        {
            NearbyMonsterBuff = new HashSet<NearbyMonsterBuff>();
            PlayerBuff = new HashSet<PlayerBuff>();
        }

        public int BuffId { get; set; }

        public ICollection<NearbyMonsterBuff> NearbyMonsterBuff { get; set; }
        public ICollection<PlayerBuff> PlayerBuff { get; set; }
    }
}
