using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class NearbyMonsterDataService
    {
        private readonly DigsiteContext _context;
        private readonly DigStateDataService _digStateDataService;

        public NearbyMonsterDataService()
        {
            _context = new DigsiteContext();
            _digStateDataService = new DigStateDataService();
        }

        public async Task Add(int playerId, Monster monster)
        {
            var digState = await _digStateDataService.Get(playerId);
            if (digState == null)
            {
                return;
            }

            await _context.NearbyMonster.AddAsync(new NearbyMonster 
            {
                DigStateId = digState.DigStateId
                , MonsterId = monster.MonsterId
                , CurrentHealth = monster.Health
            });

            _context.SaveChanges();
        }

        public async Task<List<NearbyMonster>> Get(int playerId)
        {
            var digState = await _context.DigState
                .Include(ds => ds.NearbyMonster)
                .ThenInclude(nm => nm.Monster)
                .FirstOrDefaultAsync(ds => ds.PlayerId == playerId);

            return digState.NearbyMonster.ToList();
        }
    }
}