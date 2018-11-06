using System.Threading.Tasks;
using digsite.Data;

namespace digsite.DataServices
{
    public class PlayerStateDataService
    {
        private DigsiteContext _context;

        public PlayerStateDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task AddMoney(int playerId, int amount)
        {
            var player = await _context.Player.FindAsync(playerId);
            player.PlayerState.Money += amount;
            await _context.SaveChangesAsync();
        }
    }
}