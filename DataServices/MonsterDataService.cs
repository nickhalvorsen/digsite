using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class MonsterDataService
    {
        private DigsiteContext _context;

        public MonsterDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<Monster> Get(int monsterId)
        {
            return await _context.Monster.FindAsync(monsterId);
        }
    }
}