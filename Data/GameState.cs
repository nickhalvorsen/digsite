using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class GameState
    {
        public int GameStateId { get; set; }
        public int PlayerId { get; set; }
        public byte IsDigging { get; set; }

        public Player Player { get; set; }
    }
}
