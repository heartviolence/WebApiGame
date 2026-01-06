using ServerShared.DbContexts;

namespace SampleWebApi.Service
{
    public static class DefaultGameCharacter
    {
        static public GameCharacter Create(string characterId)
        {
            return new GameCharacter() { CharacterID = characterId };
        }

        static public GameCharacter Sora()
        {
            return Create(CharacterId.Sora);
        }

        static public GameCharacter Sia()
        {
            return Create(CharacterId.Sia);
        }

        static public GameCharacter Nora()
        {
            return Create(CharacterId.Nora);
        }

        static public GameCharacter Flora()
        {
            return Create(CharacterId.Flora);
        }

    }
}
