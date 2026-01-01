using SampleWebApi.Model.DbContexts;

namespace SampleWebApi.Model.Events
{
    public interface IGameEvent
    {
        GameEvent CovertToGameEvent();
    }
}
