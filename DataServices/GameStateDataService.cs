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
                        .ThenInclude(i => i.ItemSlot)
                    .Include(gs => gs.Player.DigState.NearbyMonster)
                        .ThenInclude(nm => nm.Monster)
                    .SingleAsync(gs => gs.PlayerId == playerId);
            }
        }

        public async Task SaveGameState(GameState gameState)
        {
            using (var context = new DigsiteContext())
            {
                RemoveDeleted(context, gameState);

                context.Attach(gameState);
                IEnumerable<EntityEntry> unchangedEntities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);

                foreach(EntityEntry ee in unchangedEntities){
                    ee.State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
            }
        }

        private void RemoveDeleted(DigsiteContext context, GameState gameState)
        {
            RemoveDeletedPlayerItems(context, gameState);
            RemoveDeletedNearbyMonsters(context, gameState);
            RemoveDeletedDigState(context, gameState);
        }

        private void RemoveDeletedPlayerItems(DigsiteContext context, GameState gameState)
        {
            var existingItems = context.PlayerItem.Where(pi => pi.PlayerId == gameState.PlayerId);
            var itemsToDelete = existingItems.Where(ei => !gameState.Player.PlayerItem.Any(pi => pi.PlayerItemId == ei.PlayerItemId));
            context.PlayerItem.RemoveRange(itemsToDelete);
        }

        private void RemoveDeletedNearbyMonsters(DigsiteContext context, GameState gameState)
        {
            if (gameState.Player.DigState == null) 
            {
                return;
            }
            var existingMonsters = context.NearbyMonster.Where(nm => nm.DigState.PlayerId == gameState.PlayerId);
            var monstersToDelete = existingMonsters.Where(em => !gameState.Player.DigState.NearbyMonster.Any(nm => nm.NearbyMonsterId == em.NearbyMonsterId));
            context.NearbyMonster.RemoveRange(monstersToDelete);
        }

        private void RemoveDeletedDigState(DigsiteContext context, GameState gameState)
        {
            var contextDigState = context.DigState.SingleOrDefault(ds => ds.PlayerId == gameState.PlayerId);
            if (contextDigState != null && gameState.Player.DigState == null)
            {
                context.Remove(contextDigState);
            }
        }
    }
}