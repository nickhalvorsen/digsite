using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerItemDataService
    {
        public async Task<List<PlayerItem>> Get(int playerId)
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

                context.SaveChanges();
            }
        }
    }
}