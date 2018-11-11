using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class DigStateDataService
    {
        private DigsiteContext _context;

        public DigStateDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<DigState> GetOrCreate(int playerId)
        {
            var state = await Get(playerId);
            if (state != null)
            {
                return state;
            }

            await _context.DigState.AddAsync(new DigState
            {
                PlayerId = playerId,
                Depth = 0,
                Fuel = 100,
                IsPaused = (byte)1
            });

            _context.SaveChanges();

            return await Get(playerId);
        }

        public async Task<DigState> Get(int playerId)
        {
            return await _context.DigState.FirstOrDefaultAsync(d => d.PlayerId == playerId);
        }

        public async Task Progress(int playerId)
        {
            var state = await Get(playerId);

            state.Depth++;
            state.Fuel--;

            _context.SaveChanges();
        }

        public async Task<DigState> SetPaused(bool isPaused, int playerId)
        {
            var state = await Get(playerId);
            state.IsPaused = isPaused ? (byte)1 : (byte)0;
            _context.SaveChanges();
            return state;
        }
    }
}