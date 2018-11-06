using System.Threading.Tasks;
using digsite.Data;

namespace digsite.DataServices
{
    public class DigStateDataService
    {
        private DigsiteContext _context;

        public DigStateDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<DigState> Get(int playerId)
        {
            var player = await _context.Player.FindAsync(playerId);
            return player.DigState;
        }
    }
}