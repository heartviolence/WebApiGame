using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Games.NPCs
{
    [MessagePackObject]
    public class NPC
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public List<int> Selections { get; set; } = new();
    }
}
