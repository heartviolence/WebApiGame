using Assets.Scripts.Shared.GameDatas;
using ServerShared.DbContexts;

namespace SampleWebApi.Service
{
    public static class DefaultGameCharacter
    {
        static public GameCharacter Create(string characterId)
        {
            return new GameCharacter() { Name = characterId };
        }

        static public GameCharacter Sora()
        {
            return Create(CharacterNames.Sora);
        }

        static public GameCharacter Sia()
        {
            return Create(CharacterNames.Sia);
        }

        static public GameCharacter Nora()
        {
            return Create(CharacterNames.Nora);
        }

        static public GameCharacter Flora()
        {
            return Create(CharacterNames.Flora);
        }

    }
}
