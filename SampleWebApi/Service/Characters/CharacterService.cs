using Assets.Scripts.Shared.GameDatas;
using ServerShared.DbContexts;
using ServerShared.Events;

namespace SampleWebApi.Service.Characters
{
    public class CharacterService
    {
        public CharacterService()
        {

        }

        public UseLevelUpItemEvent UseLevelUpItem(UserAccountDetail user, GameCharacter character, int itemCount)
        {
            var item = user.GameItems.Where(i => i.Name == ItemNames.CharacterLevelUpMaterial).SingleOrDefault();
            if (item == null || item.Count < itemCount)
            {
                return null;
            }

            var modifiedItemCountInfo = new ModifiedItemCountInfo
            {
                ItemName = item.Name,
                BeforeCount = item.Count
            };

            character.EXP += itemCount * 100;
            var surplus = ProcessLevelUp(character);

            int surplusItemCount = surplus / 100;
            item.Count = item.Count - itemCount + surplusItemCount;

            modifiedItemCountInfo.AfterCount = item.Count;
            return new UseLevelUpItemEvent
            {
                UserId = user.UserId,
                CharacterName = character.Name,
                CharacterLevel = character.Level,
                CharacterEXP = character.EXP,
                ModifiedItemCountInfo = new List<ModifiedItemCountInfo> { modifiedItemCountInfo }
            };
        }

        public void PlayUseLevelUpItemEvent(UserAccountDetail user, UseLevelUpItemEvent e)
        {
            var character = user.Characters.Where(c => c.Name == e.CharacterName).First();
            character.Level = e.CharacterLevel;
            character.EXP = e.CharacterEXP;
            foreach (var modifiedItem in e.ModifiedItemCountInfo)
            {
                var gameItem = user.GameItems.Where(i => i.Name == modifiedItem.ItemName).First();
                gameItem.Count = modifiedItem.AfterCount;
            }
        }

        public CharacterRankUpEvent RankUp(UserAccountDetail user, GameCharacter character)
        {
            if (character.Rank == MaxRank() ||
                IsRankLimit(character.Rank, character.Level) == false)
            {
                return null;
            }
            var item = user.GameItems.Where(i => i.Name == ItemNames.CharacterRankUpMaterial).SingleOrDefault();
            var requireItemCount = GetRequireRankUpItemCount(character.Rank);
            if (item == null || item.Count < requireItemCount)
            {
                return null;
            }
            character.Rank += 1;

            var modifiedItemCountInfo = new ModifiedItemCountInfo
            {
                ItemName = item.Name,
                BeforeCount = item.Count,
                AfterCount = item.Count - requireItemCount
            };
            item.Count -= requireItemCount;

            var gameEvent = new CharacterRankUpEvent
            {
                UserId = user.UserId,
                CharacterName = character.Name,
                BeforeRank = character.Rank - 1,
                AfterRank = character.Rank,
                ModifiedItemCountInfo = new List<ModifiedItemCountInfo> { modifiedItemCountInfo }
            };
            return gameEvent;
        }

        public void PlayCharacterRankUpEvent(UserAccountDetail user, CharacterRankUpEvent e)
        {
            var character = user.Characters.Where(c => c.Name == e.CharacterName).SingleOrDefault();
            character.Rank = e.AfterRank;
            foreach (var itemInfo in e.ModifiedItemCountInfo)
            {
                var item = user.GameItems.Where(i => i.Name == itemInfo.ItemName).Single();
                item.Count = itemInfo.AfterCount;
            }
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
