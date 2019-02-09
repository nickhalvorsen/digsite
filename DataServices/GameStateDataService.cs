

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace digsite.DataServices
{
    public class GameStateDataService
    {
        public async Task<GameState> GetGameState(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.GameState
                    .Include(gs => gs.Player)
                    .Include(gs => gs.Player.PlayerState)
                    .Include(gs => gs.Player.DigState)
                    .Include(gs => gs.Player.PlayerItem)
                        .ThenInclude(pi => pi.Item)
                    .Include(gs => gs.Player.DigState.NearbyMonster)
                        .ThenInclude(nm => nm.Monster)
                    .SingleAsync(gs => gs.PlayerId == playerId);
            }
        }

        public async Task SaveGameState(GameState gameState)
        {




            using (var context = new DigsiteContext())
            {


                context.Attach(gameState);

                IEnumerable<EntityEntry> unchangedEntities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);

                foreach(EntityEntry ee in unchangedEntities){
                    ee.State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

return;
                context.Entry(gameState).State = EntityState.Modified;
                context.Entry(gameState.Player).State = EntityState.Modified;
                context.Entry(gameState.Player.PlayerState).State = EntityState.Modified;
                if (gameState.Player.DigState != null)
                {
                    context.Entry(gameState.Player.DigState).State = EntityState.Modified;
                }
                if (gameState.Player.DigState.NearbyMonster != null)
                {
                    context.Entry(gameState.Player.DigState.NearbyMonster).State = EntityState.Modified;
                }
                context.Entry(gameState.Player.PlayerItem).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}