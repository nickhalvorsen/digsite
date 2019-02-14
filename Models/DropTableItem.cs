using digsite.Data;

namespace digsite.Models
{
    public class DropTableItem
    {
        public DropTableItem(int itemId, int rarity)
        {
            ItemId = itemId;
            Rarity = rarity;
        }

        public int ItemId { get; set; }
        public int Rarity { get; set; }
    }
}