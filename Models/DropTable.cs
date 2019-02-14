using System;
using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.Models
{
    public class DropTable
    {
        public DropTable(List<DropTableItem> items)
        {
            _items = items;
        }
        private List<DropTableItem> _items { get; set; }
        public List<int> Roll() 
        {
            if (!_items.Any() || !RollForDrop())
            {
                return new List<int>();
            }            

            return new List<int>
            {
                RollForItem()
            };
        }

        private bool RollForDrop()
        {
            return new Random().Next(1, 10) == 5;
        }

        private int RollForItem()
        {
            var totalRarityValue = _items.Sum(i => i.Rarity);
            var roll = new Random().Next(1, totalRarityValue);

            foreach (var item in _items)
            {
                roll -= item.Rarity;
                if (roll <= 0)
                {
                    return item.ItemId;
                }
            }

            // This shouldn't happen, but if it fails,
            // just try again instead of stopping the game
            return RollForItem(); 
        }
    }
}