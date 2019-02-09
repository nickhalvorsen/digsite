

using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.GameServices
{
    public class ItemService
    {
        public List<string> ActivateItems(GameState gameState)
        {
            return gameState.Player.PlayerItem.Where(ShouldActivateItem)
            .Select(ActivateItem).ToList();
        }

        private bool ShouldActivateItem(PlayerItem playerItem)
        {
            return playerItem.CurrentCooldown == 0 && playerItem.IsEquipped == (byte)1;
        }

        private string ActivateItem(PlayerItem item)
        {
            item.CurrentCooldown = item.Item.Cooldown;
            return $"{item.Item.Name} activated.";
        }
    }
}