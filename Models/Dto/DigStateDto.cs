using System.Collections.Generic;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class DigStateDto
    {
        public bool hasDigState { get; set; }
        public int depth { get; set; }
        public int fuel { get; set; }
        public bool isPaused { get; set; }
        public List<NearbyMonsterDto> nearbyMonsters { get; set; }
    }
}