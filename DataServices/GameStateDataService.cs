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
                    .Include(gs => gs.Player.DigState.NearbyMonster)
                        .ThenInclude(nm => nm.NearbyMonsterBuff)
                    .SingleAsync(gs => gs.PlayerId == playerId);
            }
        }

        public async Task SaveGameState(GameState gameState)
        {
            var currentPlayerItemIds = gameState.Player.PlayerItem.Select(pi => pi.PlayerItemId).ToList();

            using (var context = new DigsiteContext())
            {
                context.Attach(gameState);
                RemoveDeleted(context, gameState, currentPlayerItemIds);
                IEnumerable<EntityEntry> unchangedEntities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);

                foreach(EntityEntry ee in unchangedEntities){
                    ee.State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
            }
        }

        private void RemoveDeleted(DigsiteContext context, GameState gameState, List<int> currentPlayerItemIds)
        {
            RemoveDeletedPlayerItems(context, gameState, currentPlayerItemIds);
            RemoveDeletedNearbyMonsters(context, gameState);
            RemoveDeletedDigState(context, gameState);
            RemoveDeletedPlayerBuffs(context, gameState);
            RemoveDeletedNearbyMonsterBuffs(context, gameState);
        }

        private void RemoveDeletedPlayerItems(DigsiteContext context, GameState gameState, List<int> currentPlayerItemIds)
        {
            var existingItems = context.PlayerItem.Where(pi => pi.PlayerId == gameState.PlayerId).ToList();
            var itemsToDelete = existingItems.Where(ei => !currentPlayerItemIds.Any(pi => pi == ei.PlayerItemId)).ToList();
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
            // Need to use this variable because a null gamestate.digstate turns into not null when attached by the parent function
            var gameStateDigIsNull = gameState.Player.DigState == null;
            var contextDigState = context.DigState.SingleOrDefault(ds => ds.PlayerId == gameState.PlayerId);
            if (contextDigState != null && gameStateDigIsNull)
            {
                context.DigState.Remove(contextDigState);
            }
        }

        private void RemoveDeletedPlayerBuffs(DigsiteContext context, GameState gameState)
        {

        }

        private void RemoveDeletedNearbyMonsterBuffs(DigsiteContext context, GameState gameState)
        {

        }

    }
}