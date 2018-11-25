using System;
using System.Collections.Generic;
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
    }
}