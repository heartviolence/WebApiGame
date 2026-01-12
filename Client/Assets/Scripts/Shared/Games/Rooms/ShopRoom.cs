
using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Games.Rooms
{
    [MessagePackObject]
    public class ShopRoom : IFloorRoom
    {
        [Key(0)]
        public int PowerUpCount { get; set; }
        [Key(1)]
        public List<ShopItem> Items { get; set; } = new();
    }

    [MessagePackObject]
    public class ShopItem
    {
        [Key(0)]
        public int CreditCost { get; set; }
        [Key(1)]
        public int ItemType { get; set; } // 0 노트 1 스킬카드

        [Key(2)]
        public int NoteType { get; set; }
        [Key(3)]
        public int NoteCount { get; set; }
        [Key(4)]
        public List<int> SkillCardOwnerIndexes { get; set; }

        [Key(5)]
        public bool IsSoldOut { get; set; }
    }
}
