using Assets.Scripts.Shared.Games.NPCs;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Shared.Games.Rooms
{
    [MessagePackObject]
    public class BattleRoom : IFloorRoom
    {
        [Key(0)]
        public bool IsClear { get; set; }
        [Key(1)]
        public NPC Npc { get; set; }
    }
}
