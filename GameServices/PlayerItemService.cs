using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using digsite.Data;
using digsite.DataServices;
using digsite.Models;

namespace digsite.GameServices
{
    public class PlayerItemService
    {
        private readonly PlayerItemDataService _playerItemDataService;
        private readonly ItemDataService _itemDataService;

        public PlayerItemService()
        {
            _playerItemDataService = new PlayerItemDataService();
            _itemDataService = new ItemDataService();
        }

        public async Task Unequip(int playerId, int playerItemId)
        {
            await _playerItemDataService.Unequip(playerItemId);
        }

        public async Task Equip(int playerId, int playerItemId)
        {
            if (!await CanEquipItem(playerItemId))
            {
                return;
            }

            await _playerItemDataService.Equip(playerItemId);
        }

        private async Task<bool> CanEquipItem(int playerItemId)
        {
            var playerItem = await _playerItemDataService.Get(playerItemId);
            var itemSlotId = (await _itemDataService.Get(playerItem.ItemId)).ItemSlotId;
            var playerItems = await _playerItemDataService.GetPlayer(playerItem.PlayerId);

            return playerItems.Any(pi => pi.Item.ItemSlotId == itemSlotId);
        }

        public async Task<List<string>> ActivateItems(int playerId)
        {

            var results = new List<string>();
            
            var itemsToActivate = await _playerItemDataService.GetItemsToActivate(playerId);
            foreach (var item in itemsToActivate)
            {
                var message = await ActivateItem(item);
                results.Add(message);
            }
            await _playerItemDataService.CooldownTick(playerId);

            return results;
        }

        private async Task<string> ActivateItem(PlayerItem item)
        {
            switch (item.ItemId)
            {
                case (int)ItemId.AmuletOfBurning:
                case (int)ItemId.AmuletOfFoulOdor:
                    return await DealDamage(item);
                default:
                    return string.Empty;
            }
        }

        private async Task<string> DealDamage(PlayerItem item)
        {
            await _playerItemDataService.PutOnCooldown(item);
            return $"Your {item.Item.Name} deals some damage.";
        }
    }
}