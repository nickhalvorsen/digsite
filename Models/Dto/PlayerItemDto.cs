using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class PlayerItemDto
    {
        public string name;
        public int itemCategoryId;
    }
}