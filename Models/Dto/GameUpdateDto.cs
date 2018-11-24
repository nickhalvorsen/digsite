using System.Collections.Generic;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class GameUpdateDto
    {
        public PlayerState playerState;
        public DigStateDto digState;
        public List<PlayerItemDto> itemState;
    }
}