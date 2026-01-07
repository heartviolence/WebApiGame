using Assets.Scripts.Shared.GameDatas;
using SampleWebApi.Model.Characters;
using System.Collections.ObjectModel;

namespace SampleWebApi.Service
{
    public class GameCharacterDataProvider : IGameCharacterDataProvider
    {
        public Dictionary<string, GameCharacterData> GameCharacterData { get; private set; }

        public GameCharacterDataProvider()
        {
            GameCharacterData = new Dictionary<string, GameCharacterData>();
            Initialize();
        }

        void Initialize()
        {
            GameCharacterData.Add(CharacterNames.Sora, new GameCharacterData()
            {
                characterCode = CharacterNames.Sora,
                Type = GameCharacterType.Dealer,
            });

            GameCharacterData.Add(CharacterNames.Sia, new GameCharacterData()
            {
                characterCode = CharacterNames.Sia,
                Type = GameCharacterType.Support,
            });

            GameCharacterData.Add(CharacterNames.Nora, new GameCharacterData()
            {
                characterCode = CharacterNames.Nora,
                Type = GameCharacterType.Balance,
            });

            GameCharacterData.Add(CharacterNames.Flora, new GameCharacterData()
            {
                characterCode = CharacterNames.Flora,
                Type = GameCharacterType.Dealer,
            });
        }
    }
}
