using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GameItemType Type { get; set; }
        public int Count { get; set; }
    }

    public enum GameItemType
    {
        Material = 0,
        Usable
    }
}
