using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Games.SkillCards
{
    [MessagePackObject]
    public class SkillCard
    {
        [Key(0)]
        public string OwnerName { get; set; }
        [Key(1)]
        public string CardName { get; set; }
        [Key(2)]
        public int Level { get; set; }
        [Key(3)]
        public int Type { get; set; } 
    }

    [MessagePackObject]
    public class SkillCardBook
    {
        [Key(0)]
        public string OwnerName;
        [Key(1)]
        public List<SkillCard> Cards;
    }
}
