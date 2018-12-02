using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using digsite.DataServices;

namespace digsite.GameServices
{
    public class DigTickService
    {
        private readonly DigStateDataService _digStateDataService;
        private readonly MonsterDataService _monsterDataService;
        private readonly NearbyMonsterDataService _nearbyMonsterDataService;
        private readonly PlayerItemDataService _playerItemDataService;
        private readonly ItemDataService _itemDataService;

        public DigTickService()
        {
            _digStateDataService = new DigStateDataService();
            _monsterDataService = new MonsterDataService();
            _nearbyMonsterDataService = new NearbyMonsterDataService();
            _playerItemDataService = new PlayerItemDataService();
            _itemDataService = new ItemDataService();
        }

        public async Task<List<string>> Tick(int playerId)
        {
            var messages = new List<string>();

            await _digStateDataService.Progress(playerId);
            if (new Random().Next(1, 10) == 4)
            {
                var msg = await MonsterApproach(playerId);
                messages.Add(msg);
            }
            if (new Random().Next(1, 100) > 70)
            {
                var msg = await GiveRandomItem(playerId);
                messages.Add(msg);
            }

            var monsterMessages = await HandleMonsterAttacks(playerId);
            messages.AddRange(monsterMessages);
            
            messages.Add("you dig a little deeper.");

            return messages;
        }

        private async Task<string> MonsterApproach(int playerId)
        {
            var randomMonsterId = new Random().Next(1, 3);
            var randomMonster = await _monsterDataService.Get(randomMonsterId);
            await _nearbyMonsterDataService.Add(playerId, randomMonster); 
            return $"A {randomMonster.Name} approaches.";
        }

        private async Task<string> GiveRandomItem(int playerId)
        {
            var itemId = new Random().Next(1, 7);
            await _playerItemDataService.Give(playerId, itemId);
            var item = await _itemDataService.Get(itemId);
            return $"You found a {item.Name}.";
        }

        private async Task<List<string>> HandleMonsterAttacks(int playerId)
        {
            var digState = await _digStateDataService.Get(playerId);
            await _nearbyMonsterDataService.CooldownTick(digState.DigStateId);
            var monsters = await _nearbyMonsterDataService.Get(playerId);
            var attackers = monsters.Where(m => m.CurrentAttackCooldown < 0);


            var messages = new List<string>();
            foreach (var attacker in attackers) 
            {
                var damage = new Random().Next(1, attacker.Monster.Attack);
                var message = $"{attacker.Monster.Name} hits for {damage}.";
                messages.Add(message);
                await _digStateDataService.TakeDamage(playerId, damage);
            }

            await _nearbyMonsterDataService.CooldownReset(digState.DigStateId);

            return messages;
        }
    }
}