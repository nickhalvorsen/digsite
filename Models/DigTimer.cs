using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace digsite.Models
{
    public class DigTimer
    {
        public int PlayerId { get; set; }
        public Timer Timer { get; set; }
        public Func<int, List<string>, Task> Callback { get; set; }
    }
}