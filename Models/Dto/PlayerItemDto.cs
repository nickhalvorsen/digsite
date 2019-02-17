using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerItemDto
    {
        public int playerItemId;
        public int itemId;
        public string name;
        public int itemCategoryId;
        public bool isEquipped; 
        public int itemSlotId;
        public int currentCooldown;
        public string slotName;
        public string description;
        public int upgradeLevel;
    }
}