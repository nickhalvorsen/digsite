﻿using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class NearbyMonsterBuff
    {
        public int NearbyMonsterBuffId { get; set; }
        public int NearbyMonsterId { get; set; }
        public int BuffId { get; set; }
        public int RemainingDuration { get; set; }

        public Buff Buff { get; set; }
        public NearbyMonster NearbyMonster { get; set; }
    }
}
