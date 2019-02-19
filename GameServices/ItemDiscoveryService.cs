using System.Collections.Generic;
using System.Linq;
using digsite.Data;
using digsite.DataServices;
using digsite.Models;

namespace digsite.GameServices
{
    public class ItemDiscoveryService
    {
        public List<string> FindItems(GameState gameState)
        {
            var dropTable = GetDropTable(gameState);
            var foundItemIds = dropTable.Roll();
            var foundItems = GetFoundItems(foundItemIds).ToList();

            AddItems(gameState, foundItems);
            return foundItems.Select(fi => $"You found a {fi.Name}!").ToList();
        }

        private DropTable GetDropTable(GameState gameState)
        {
            var items = new List<DropTableItem> 
            {
                new DropTableItem((int)ItemId.RustedTableLeg, 20)
                , new DropTableItem((int)ItemId.BrassCandlestick, 20)
                , new DropTableItem((int)ItemId.CowboyHat, 40)
                , new DropTableItem((int)ItemId.BackwardsBallcap, 40)
                , new DropTableItem((int)ItemId.AmuletOfBurning, 40)
                , new DropTableItem((int)ItemId.AmuletOfFoulOdor, 40)
            };

            return new DropTable(items);
        }

        private IEnumerable<Item> GetFoundItems(List<int> itemIds)
        {
            var itemDataService = new ItemDataService();

            foreach (var itemId in itemIds)
            {
                var item = itemDataService.Get(itemId);
                if (item == null)
                {
                    continue;
                }
                yield return item;
            }
        }

        private void AddItems(GameState gameState, List<Item> items)
        {
            items.ForEach(i => AddItem(gameState, i));
        }

        private void AddItem(GameState gameState, Item item)
        {
            var playerItem = GeneratePlayerItemFromTemplate(item);
            gameState.Player.PlayerItem.Add(playerItem);
        }

        private PlayerItem GeneratePlayerItemFromTemplate(Item item)
        {
            return new PlayerItem
            {
                CurrentCooldown = 0,
                IsEquipped = 0,
                ItemId = item.ItemId,
                UpgradeLevel = 1
            };
        }
    }
}