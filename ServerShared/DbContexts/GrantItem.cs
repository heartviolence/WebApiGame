using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class GrantItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public List<GameItem> Items { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
