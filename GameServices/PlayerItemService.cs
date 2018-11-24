using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using digsite.DataServices;

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
    }
}