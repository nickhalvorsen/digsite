using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class NearbyMonsterDataService
    {
        private readonly DigStateDataService _digStateDataService;
        private readonly MonsterDataService _monsterDataService;

        public NearbyMonsterDataService()
        {
            _digStateDataService = new DigStateDataService();
            _monsterDataService = new MonsterDataService();
        }

        public async Task Add(int playerId, Monster monster)
        {
            var digState = await _digStateDataService.Get(playerId);
            if (digState == null)
            {
                return;
            }

            using (var context = new DigsiteContext())
            {
                await context.NearbyMonster.AddAsync(new NearbyMonster 
                {
                    DigStateId = digState.DigStateId
                    , MonsterId = monster.MonsterId
                    , CurrentHealth = monster.Health
                    , CurrentAttackCooldown = 2
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<NearbyMonster>> Get(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var digState = await context.DigState
                    .Include(ds => ds.NearbyMonster)
                    .ThenInclude(nm => nm.Monster)
                    .FirstOrDefaultAsync(ds => ds.PlayerId == playerId);

                return digState.NearbyMonster.ToList();
            }
        }

        public async Task Clear(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var digState = await context
                .DigState
                .SingleAsync(ds => ds.PlayerId == playerId);

                digState.NearbyMonster.Clear();

                await context.SaveChangesAsync();
            }
        }

        public async Task CooldownTick(int digStateId)
        {
            using (var context = new DigsiteContext())
            {
                var monsters = context.NearbyMonster.Where(nm => nm.DigStateId == digStateId);
                foreach (var monster in monsters)
                {
                    monster.CurrentAttackCooldown--;
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task CooldownReset(int digStateId)
        {
            using (var context = new DigsiteContext())
            {
                var monsters = context.NearbyMonster.Where(nm => nm.DigStateId == digStateId);
                foreach (var monster in monsters)
                {
                    if (monster.CurrentAttackCooldown < 0)
                    {
                        var baseMonster = await _monsterDataService.Get(monster.MonsterId);
                        monster.CurrentAttackCooldown = baseMonster.AttackRate;
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}