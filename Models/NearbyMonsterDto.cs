using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class NearbyMonsterDto
    {
        public string name { get; set; }
        public int maxHealth { get; set; }
        public int currentHealth { get; set; }
    }
}