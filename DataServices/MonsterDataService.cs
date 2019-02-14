using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class MonsterDataService
    {
        public Monster Get(int monsterId)
        {
            using (var context = new DigsiteContext())
            {
                return context.Monster.Find(monsterId);
            }
        }
    }
}