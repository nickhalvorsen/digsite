using System;
using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.GameServices
{
    public class PlayerItemUpgradeService
    {
        public List<string> UpgradeItem(GameState gameState, int playerItemId1, int playerItemId2)
        {
            var playerItem1 = gameState.Player.PlayerItem.FirstOrDefault(pi => pi.PlayerItemId == playerItemId1);
            var playerItem2 = gameState.Player.PlayerItem.FirstOrDefault(pi => pi.PlayerItemId == playerItemId2);
            if (playerItem1 == null || playerItem2 == null)
            {
                return new List<string>();
            }

            PlayerItem itemToUpgrade;
            PlayerItem itemToConsume;

            if (playerItem1.UpgradeLevel >= playerItem2.UpgradeLevel)
            {
                itemToUpgrade = playerItem1;
                itemToConsume = playerItem2;
            }
            else
            {
                itemToUpgrade = playerItem2;
                itemToConsume = playerItem1;
            }

            CombineItems(gameState, itemToUpgrade, itemToConsume);

            if (WasUpgradeSuccessful())
            {
                itemToUpgrade.UpgradeLevel++;
            }
            else
            {
                itemToUpgrade.UpgradeLevel--;
            }

            return new List<string>();
        }

        private void CombineItems(GameState gameState, PlayerItem itemToUpgrade, PlayerItem itemToConsume)
        {
            if (itemToConsume.IsEquipped == (byte)1)
            {
                itemToUpgrade.IsEquipped = (byte)1;
            }

            gameState.Player.PlayerItem.Remove(itemToConsume);
            itemToUpgrade.CurrentCooldown = 0;
        }

        private bool WasUpgradeSuccessful()
        {
            return new Random().Next(1, 2) == 1;
        }
    }
}