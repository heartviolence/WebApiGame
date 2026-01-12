

namespace Assets.Scripts.Shared.Games.Rooms
{

    [MessagePack.Union(0, typeof(BattleRoom))]
    [MessagePack.Union(1, typeof(NPCRoom))]
    [MessagePack.Union(2, typeof(ShopRoom))]
    public interface IFloorRoom
    {
    }
}
