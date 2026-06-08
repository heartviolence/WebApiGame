using ServerShared.DbContexts;

namespace ServerShared.Events
{
    public interface IGameEvent
    {
        GameEvent CovertToGameEvent();

        public string EventVersion { get; }

        public DateTime TimeStamp { get; }
    }
}
