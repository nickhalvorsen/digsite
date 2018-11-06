using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class DigState
    {
        public int DigStateId { get; set; }
        public int PlayerId { get; set; }
        public int Depth { get; set; }
        public int Fuel { get; set; }

        public Player Player { get; set; }
    }
}
