using MessagePack;

namespace SampleWebApi.Service.Games.NPCs
{
    [MessagePackObject]
    public class NPC
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public List<int> Selections { get; set; } = new();
        public void Initialize(string name)
        {
            Selections = new List<int>();
            Selections.Add(0);
            Selections.Add(1);
        }

        public void Select(GameState gamestate, int index)
        {
            if (Selections.Count <= index)
            {
                return;
            }
            var rewardType = this.Selections[index];

            switch (rewardType)
            {
                case 0:
                    gamestate.Notes[0] += 1;
                    break;
                case 1:
                    gamestate.Notes[1] += 2;
                    break;
            }

            Selections.Clear();
        }
    }
}
