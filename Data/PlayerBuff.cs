using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class PlayerBuff
    {
        public int PlayerBuffId { get; set; }
        public int PlayerId { get; set; }
        public int BuffId { get; set; }
        public int RemainingDuration { get; set; }

        public Buff Buff { get; set; }
        public Player Player { get; set; }
    }
}
