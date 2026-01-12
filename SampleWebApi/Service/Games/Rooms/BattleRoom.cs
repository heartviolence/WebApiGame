using MessagePack;
using SampleWebApi.Service.Games.NPCs;

namespace SampleWebApi.Service.Games.Rooms
{
    [MessagePackObject]
    public class BattleRoom : IFloorRoom
    {
        [Key(0)]
        public bool IsClear { get; set; }
        [Key(1)]
        public NPC Npc { get; set; }
        public void BattleEnd(GameState gameState)
        {
            if (IsClear)
            {
                return;
            }
            IsClear = true;
            Npc = new NPC();
            Npc.Initialize(string.Empty);
            var noteCount = Random.Shared.Next(1, 3);
            var noteType = Random.Shared.Next(0, gameState.Notes.Count - 1);
            gameState.Notes[noteType] += (int)noteCount;
            List<int> ownerIndexes = new List<int>();
            for(int i=0;i<gameState.SkillCardBooks.Count;i++)
            {
                ownerIndexes.Add(i);
            }
            gameState.CardSelector.AddNewCards(gameState, ownerIndexes);
        }

        public void BuyItem(GameState gameState, int index)
        {

        }

        public bool CanGoNextFloor()
        {
            return IsClear;
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
