using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class UserMail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public List<GameItem> Items { get; set; } = new();
    }
}
