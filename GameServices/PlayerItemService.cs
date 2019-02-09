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
        private readonly ItemDataService _itemDataService;

        public PlayerItemService()
        {
            _itemDataService = new ItemDataService();
        }

        public void Unequip(GameState gameState, int playerItemId)
        {
            var item = gameState.Player.PlayerItem.FirstOrDefault(pi => pi.PlayerItemId == playerItemId);
            if (item == null)
            {
                return;
            }
            item.IsEquipped = (byte)0;
        }

        public void Equip(GameState gameState, int playerItemId)
        {
            var itemToEquip = gameState.Player.PlayerItem.FirstOrDefault(pi => pi.PlayerItemId == playerItemId);
            if (itemToEquip == null)
            {
                return;
            }
            
            var equippedInSameSlot = gameState.Player.PlayerItem.Where(pi => pi.Item.ItemSlotId == itemToEquip.Item.ItemSlotId).ToList();
            equippedInSameSlot.ForEach(i => i.IsEquipped = (byte)0);
            itemToEquip.IsEquipped = (byte)1;
        }
    }
}