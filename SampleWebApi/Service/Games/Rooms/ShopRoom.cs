using Assets.Scripts.Shared.Games.Rooms;
using MessagePack;

namespace SampleWebApi.Service.Games.Rooms
{
    [MessagePackObject]
    public class ShopRoom : IFloorRoom
    {
        [Key(0)]
        public int PowerUpCount { get; set; }
        [Key(1)]
        public List<ShopItem> Items { get; set; } = new();

        public void Reroll(GameState gamestate)
        {
            Items.Clear();
            Items.Add(CreateShopItem(gamestate));
            Items.Add(CreateShopItem(gamestate));
            Items.Add(CreateShopItem(gamestate));
            Items.Add(CreateShopItem(gamestate));
        }

        ShopItem CreateShopItem(GameState gamestate)
        {
            var skillCardOwnerIndexes = new List<int>();

            for (int i = 0; i < gamestate.SkillCardBooks.Count; i++)
            {
                skillCardOwnerIndexes.Add(i);
            }

            return new ShopItem()
            {
                CreditCost = 100,
                ItemType = 1,
                NoteType = 0,
                NoteCount = 0,
                SkillCardOwnerIndexes = skillCardOwnerIndexes,
                IsSoldOut = false
            };
        }

        public void BattleEnd(GameState gameState)
        {

        }

        public void BuyItem(GameState gameState, int index)
        {
            var item = Items[index];

            if (item.CreditCost <= gameState.Credit)
            {
                gameState.Credit -= item.CreditCost;
                if (item.ItemType == 1)
                {
                    gameState.CardSelector.AddNewCards(gameState, item.SkillCardOwnerIndexes);
                }
            }
        }

        public bool CanGoNextFloor()
        {
            return true;
        }

        public void PowerUp(GameState gamestate)
        {
            var skillCardOwnerIndexes = new List<int>();
            for (int i = 0; i < gamestate.SkillCardBooks.Count; i++)
            {
                skillCardOwnerIndexes.Add(i);

            }
            var cost = GetPowerUpCost(PowerUpCount);
            if (cost <= gamestate.Credit)
            {
                PowerUpCount++;
                gamestate.Credit -= cost;
                gamestate.CardSelector.AddPowerUpCards(gamestate, skillCardOwnerIndexes);
            }
        }

        private int GetPowerUpCost(int powerUpCount)
        {
            return 100;
        }
        public void SelectNPC(GameState gameState, int index)
        {

        }

        public void SetShopItem(List<ShopItem> items)
        {
            this.Items = items;
        }
    }
}
