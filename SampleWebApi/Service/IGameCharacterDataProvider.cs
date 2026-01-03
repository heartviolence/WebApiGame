using SampleWebApi.Model.Characters;

namespace SampleWebApi.Service
{
    public interface IGameCharacterDataProvider
    {
        public Dictionary<string, GameCharacterData> GameCharacterData { get; }
    }
}
