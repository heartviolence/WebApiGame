using Assets.Scripts.Shared.Games.NPCs;
using Assets.Scripts.Shared.Games.Rooms;
using Assets.Scripts.Shared.Games.Selectors;
using Assets.Scripts.Shared.Games.SkillCards;
using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Games
{
    [MessagePackObject]
    public class GameState
    {
        [Key(0)]
        public int Credit { get; set; }
        [Key(1)]
        public List<int> Notes { get; set; } = new();

        [Key(2)]
        public List<SkillCardBook> SkillCardBooks { get; set; } = new();

        [Key(3)]
        public int Currentfloor { get; set; }
        [Key(4)]
        public IFloorRoom CurrentRoom { get; set; }
        [Key(5)]
        public CardSelector CardSelector { get; set; } = new();
    }

}
