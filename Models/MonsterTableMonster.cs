using digsite.Data;

namespace digsite.Models
{
    public class MonsterTableMonster
    {
        public MonsterTableMonster(int monsterId, int rarity)
        {
            MonsterId = monsterId;
            Rarity = rarity;
        }

        public int MonsterId { get; set; }
        public int Rarity { get; set; }
    }
}