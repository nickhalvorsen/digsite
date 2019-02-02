using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerItemDataService
    {

        public async Task<PlayerItem> Get(int playerItemId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.PlayerItem
                    .Include(pi => pi.Item)
                    .Where(pi => pi.PlayerItemId == playerItemId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<PlayerItem>> GetPlayer(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.PlayerItem
                    .Include(pi => pi.Item)
                    .Where(pi => pi.PlayerId == playerId)
                    .ToListAsync();
            }
        }

        public async Task Give(int playerId, int itemId)
        {
            using (var context = new DigsiteContext())
            {
                var playerItem = new PlayerItem
                {
                    PlayerId = playerId
                    , ItemId = itemId
                };

                await context.PlayerItem.AddAsync(playerItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task Equip(int playerItemId)
        {
            using (var context = new DigsiteContext())
            {
                var item = await context.PlayerItem.FindAsync(playerItemId);
                item.IsEquipped = (byte)1;
                await context.SaveChangesAsync();
            }
        }

        public async Task Unequip(int playerItemId)
        {
            using (var context = new DigsiteContext())
            {
                var item = await context.PlayerItem.FindAsync(playerItemId);
                item.IsEquipped = (byte)0;
                await context.SaveChangesAsync();
            }
        }

        public async Task CooldownTick(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var items = await context.PlayerItem.Where(pi => pi.PlayerId == playerId).ToListAsync();
                foreach (var item in items)
                {
                    if (item.CurrentCooldown > 0)
                    {
                        item.CurrentCooldown--;
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<PlayerItem>> GetItemsToActivate(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.PlayerItem.Where(pi => pi.PlayerId == playerId && pi.CurrentCooldown == 0).ToListAsync(); 
            }
        }

        public async Task PutOnCooldown(PlayerItem item)
        {
            using (var context = new DigsiteContext())
            {
                item.CurrentCooldown = 
                return await context.PlayerItem.Where(pi => pi.PlayerId == playerId && pi.CurrentCooldown == 0).ToListAsync(); 
            }
        }
    }
}