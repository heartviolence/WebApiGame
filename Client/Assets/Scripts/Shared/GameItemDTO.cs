using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Shared
{
    public class GameItemDTO
    {
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
