using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class PlayerState
    {
        public int PlayerStateId { get; set; }
        public int Money { get; set; }
        public int PlayerId { get; set; }

        public Player Player { get; set; }
    }
}
