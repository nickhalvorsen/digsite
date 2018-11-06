using System;
using System.Collections.Generic;

namespace digsite.Data
{
    public partial class Player
    {
        public int PlayerId { get; set; }
        public string Email { get; set; }

        public DigState DigState { get; set; }
        public GameState GameState { get; set; }
        public PlayerState PlayerState { get; set; }
    }
}
