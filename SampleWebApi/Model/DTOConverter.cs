using Assets.Scripts.Shared;
using ServerShared.DbContexts;

namespace SampleWebApi.Model
{
    public static class DTOConverter
    {
        public static UserInfoDTO DTO(this UserInfo x)
        {
            return new UserInfoDTO()
            {
                Id = x.Id,
                Username = x.Username,
                Nickname = x.Nickname,
                Characters = x.Characters.ConvertAll(x => x.DTO()),
                Crystal = x.Crystal,
                RequestMissions = x.RequestMissions.ConvertAll(x => x.DTO()),
                GameItems = x.GameItems.ConvertAll(x => x.DTO()),
                Records = x.Records.ConvertAll(x => x.DTO())
            };
        }

        public static GameCharacterDTO DTO(this GameCharacter x)
        {
            return new GameCharacterDTO()
            {
                Name = x.Name,
                Level = x.Level,
                EXP = x.EXP,
                A_Skill_Level = x.A_Skill_Level,
                B_Skill_Level = x.B_Skill_Level,
                StarLevel = x.StarLevel,
                Rank = x.Rank,
            };
        }

        public static RequestMissionDTO DTO(this RequestMission x)
        {
            return new RequestMissionDTO()
            {
                MissionCode = x.MissionCode,
                StartTime = x.StartTime,
            };
        }

        public static GameItemDTO DTO(this GameItem x)
        {
            return new GameItemDTO()
            {
                Count = x.Count,
                Name = x.Name,
                Type = (Assets.Scripts.Shared.GameItemType)(int)x.Type
            };
        }

        public static RecordItemDTO DTO(this RecordItem x)
        {
            return new RecordItemDTO()
            {
                Name = x.Name,
                StarLevel = x.StarLevel,
            };
        }
    }
}
