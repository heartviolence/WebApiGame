namespace SampleWebApi.Service.Games.Rooms
{
    [MessagePack.Union(0, typeof(BattleRoom))]
    [MessagePack.Union(1, typeof(NPCRoom))]
    [MessagePack.Union(2, typeof(ShopRoom))]
    public interface IFloorRoom
    {
        bool CanGoNextFloor();

        void BattleEnd(GameState gameState);

        void SelectNPC(GameState gameState, int index);

        void BuyItem(GameState gameState, int index);

        void PowerUp(GameState gameState);
    }
}
