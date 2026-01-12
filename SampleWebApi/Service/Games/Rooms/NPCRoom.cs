using MessagePack;
using SampleWebApi.Service.Games.NPCs;

namespace SampleWebApi.Service.Games.Rooms
{
    [MessagePackObject]
    public class NPCRoom : IFloorRoom
    {
        [Key(0)]
        public NPC Npc { get; set; }

        public void BattleEnd(GameState gameState)
        {

        }

        public void BuyItem(GameState gameState, int index)
        {

        }

        public bool CanGoNextFloor()
        {
            return true;
        }

        public void PowerUp(GameState gameState)
        {

        }

        public void SelectNPC(GameState gameState, int index)
        {
            Npc.Select(gameState, index);
        }
    }
}
