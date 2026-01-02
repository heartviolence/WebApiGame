using SampleWebApi.Model.Characters;
using SampleWebApi.Model.DbContexts;
using System.Collections.ObjectModel;

namespace SampleWebApi.Service
{
    public class GameCharacterDataProvider
    {
        public Dictionary<string, GameCharacterData> GameCharacterData { get; private set; }

        public GameCharacterDataProvider()
        {
            GameCharacterData = new Dictionary<string, GameCharacterData>();
            Initialize();
        }

        void Initialize()
        {
            GameCharacterData.Add(CharacterId.Sora, new GameCharacterData()
            {
                characterCode = CharacterId.Sora,
                Type = GameCharacterType.Dealer,
            });

            GameCharacterData.Add(CharacterId.Sia, new GameCharacterData()
            {
                characterCode = CharacterId.Sia,
                Type = GameCharacterType.Support,
            });

            GameCharacterData.Add(CharacterId.Nora, new GameCharacterData()
            {
                characterCode = CharacterId.Nora,
                Type = GameCharacterType.Balance,
            });

            GameCharacterData.Add(CharacterId.Flora, new GameCharacterData()
            {
                characterCode = CharacterId.Flora,
                Type = GameCharacterType.Dealer,
            });
        }
    }
}
