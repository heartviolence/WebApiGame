using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public interface IGameEvent
    {
        GameEvent CovertToGameEvent();
    }
}
