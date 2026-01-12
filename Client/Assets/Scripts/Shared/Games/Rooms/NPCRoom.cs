using Assets.Scripts.Shared.Games.NPCs;
using MessagePack;

namespace Assets.Scripts.Shared.Games.Rooms
{
    [MessagePackObject]
    public class NPCRoom : IFloorRoom
    {
        [Key(0)]
        public NPC Npc { get; set; }
    }
}
