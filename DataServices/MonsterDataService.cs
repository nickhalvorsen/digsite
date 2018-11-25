using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class MonsterDataService
    {
        public async Task<Monster> Get(int monsterId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.Monster.FindAsync(monsterId);
            }
        }
    }
}