using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerStateDataService
    {
        public async Task<PlayerState> GetPlayerState(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.PlayerState.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            }
        }

        public async Task AddMoney(int playerId, int amount)
        {
            using (var context = new DigsiteContext())
            {
                var playerState = await context.PlayerState.SingleAsync(ps => ps.PlayerId == playerId);
                playerState.Money += amount;
                await context.SaveChangesAsync();
            }
        }
    }
}