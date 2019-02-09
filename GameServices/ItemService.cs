

using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.GameServices
{
    public class ItemService
    {
        public List<string> ActivateItems(GameState gameState)
        {
            var itemsToActivate = GetItemsToActivate(gameState);
            return itemsToActivate.Select(ActivateItem).ToList();
        }

        private List<PlayerItem> GetItemsToActivate(GameState gameState)
        {
            return gameState.Player.PlayerItem.Where(pi => pi.CurrentCooldown == 0).ToList();
        }

        private string ActivateItem(PlayerItem item)
        {
            // todo scaffold database to add this column
            //item.CurrentCooldown = item.Item.Cooldown;
            return $"{item.Item.Name} activated.";
        }
    }
}