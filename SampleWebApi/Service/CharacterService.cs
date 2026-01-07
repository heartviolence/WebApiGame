using Assets.Scripts.Shared.GameDatas;
using ServerShared.DbContexts;

namespace SampleWebApi.Service
{
    public class CharacterService
    {
        public CharacterService()
        {

        }

        public void UseLevelUpItem(UserInfo user, GameCharacter character, int itemCount)
        {
            var item = user.GameItems.Where(i => i.Name == ItemNames.CharacterLevelUpMaterial).SingleOrDefault();
            if (item == null || item.Count < itemCount)
            {
                return;
            }
            character.EXP += itemCount * 100;
            var surplus = ProcessLevelUp(character);

            int surplusItemCount = surplus / 100;
            item.Count = item.Count - itemCount + surplusItemCount;
        }

        public void RankUp(UserInfo user, GameCharacter character)
        {
            if (character.Rank == MaxRank() ||
                IsRankLimit(character.Rank, character.Level) == false)
            {
                return;
            }
            var item = user.GameItems.Where(i => i.Name == ItemNames.CharacterRankUpMaterial).SingleOrDefault();
            var requireItemCount = GetRequireRankUpItemCount(character.Rank);
            if (item == null || item.Count < requireItemCount)
            {
                return;
            }
            character.Rank += 1;
            item.Count -= requireItemCount;
        }

        int ProcessLevelUp(GameCharacter character)
        {
            while (true)
            {
                var LevelUpExp = GetRequireLevelUpExp(character.Level);
                if (IsRankLimit(character.Rank, character.Level))
                {
                    var surplus = character.EXP;
                    character.EXP = 0;
                    return surplus;
                }
                if (character.EXP >= LevelUpExp)
                {
                    character.EXP -= LevelUpExp;
                    character.Level += 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        bool IsRankLimit(int rank, int level)
        {
            return (rank + 1) * 10 == level;
        }

        int GetRequireLevelUpExp(int Level)
        {
            return 100;
        }

        int GetRequireRankUpItemCount(int rank)
        {
            return rank + 1;
        }

        int MaxRank()
        {
            return 6;
        }
    }
}
