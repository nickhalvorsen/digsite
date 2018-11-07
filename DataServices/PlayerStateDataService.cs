using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerStateDataService
    {
        private DigsiteContext _context;

        public PlayerStateDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<PlayerState> GetPlayerState(int playerId)
        {
           return await _context.PlayerState.FirstOrDefaultAsync(p => p.PlayerId == playerId);
        }

        public async Task AddMoney(int playerId, int amount)
        {
            var player = await _context.Player.Include("PlayerState").SingleAsync(p => p.PlayerId == playerId);
            player.PlayerState.Money += amount;
            await _context.SaveChangesAsync();
        }
    }
}