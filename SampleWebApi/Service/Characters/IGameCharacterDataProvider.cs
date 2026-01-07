using SampleWebApi.Model.Characters;

namespace SampleWebApi.Service.Characters
{
    public interface IGameCharacterDataProvider
    {
        public Dictionary<string, GameCharacterData> GameCharacterData { get; }
    }
}
