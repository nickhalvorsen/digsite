using System.Collections.Generic;
using System.Linq;
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

        public void CooldownTick(GameState gameState)
        {
            gameState.Player.PlayerItem.ToList().ForEach(pi => pi.CurrentCooldown--);
        }

        public List<string> ActivateItems(GameState gameState)
        {
            var itemsToActivate = gameState.Player.PlayerItem
            .Where(ShouldActivateItem).ToList();

            var messages = new List<string>();

            for (var i = 0; i < itemsToActivate.Count(); i++)
            {
                messages.Add(ActivateItem(gameState, itemsToActivate[i]));
            }

            return messages;
        }

        private bool ShouldActivateItem(PlayerItem playerItem)
        {
            return playerItem.CurrentCooldown <= 0 && playerItem.IsEquipped == (byte)1;
        }

        private string ActivateItem(GameState gameState, PlayerItem item)
        {
            item.CurrentCooldown = item.Item.Cooldown;
            switch (item.ItemId)
            {
                case (int)ItemId.AmuletOfBurning:
                    return AmuletOfBurningActivate(gameState);
                case (int)ItemId.AmuletOfFoulOdor:
                    return AmuletOfFoulOdorActivate(gameState);
            }

            return $"{item.Item.Name} activated.";
        }

        private string AmuletOfBurningActivate(GameState gameState)
        {
            var damage = 1;
            gameState.Player.DigState.NearbyMonster.ToList().ForEach(m => m.CurrentHealth -= damage);
            return $"Your amulet of burning hits for {damage} (x{gameState.Player.DigState.NearbyMonster.Count}).";
        }

        private string AmuletOfFoulOdorActivate(GameState gameState)
        {
            var damage = 3;
            var monster = gameState.Player.DigState.NearbyMonster.FirstOrDefault();
            if (monster == null)
            {
                return "";
            }
            monster.CurrentHealth -= damage;
            return $"Your amulet of foul odor hits {monster.Monster.Name} for {damage}.";
        }
    }
}