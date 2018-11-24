using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class ItemDataService
    {
        private readonly DigsiteContext _context;

        public ItemDataService()
        {
            _context = new DigsiteContext();
        }

        public async Task<Item> Get(int itemId)
        {
            return await _context.Item.FindAsync(itemId);
        }
    }
}