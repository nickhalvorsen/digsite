using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerItemDataService
    {
        private readonly DigsiteContext _context;

        public PlayerItemDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<PlayerItem> Get(int playerItemId)
        {
            return await _context.PlayerItem
                .Include(pi => pi.Item)
                .Where(pi => pi.PlayerItemId == playerItemId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PlayerItem>> GetPlayer(int playerId)
        {
            return await _context.PlayerItem
                .Include(pi => pi.Item)
                .Where(pi => pi.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task Give(int playerId, int itemId)
        {
            var playerItem = new PlayerItem
            {
                PlayerId = playerId
                , ItemId = itemId
            };

            await _context.PlayerItem.AddAsync(playerItem);

            _context.SaveChanges();
        }

        public async Task Equip(int playerItemId)
        {
            var item = await _context.PlayerItem.FindAsync(playerItemId);
            item.IsEquipped = (byte)1;
        }

        public async Task Unequip(int playerItemId)
        {
            var item = await _context.PlayerItem.FindAsync(playerItemId);
            item.IsEquipped = (byte)0;
        }
    }
}