using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Characters;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Model.Events;
using SampleWebApi.Model.Items;
using SampleWebApi.Model.RequestMissions;

namespace SampleWebApi.Service
{
    public class RequestMissionRepository
    {
        RequestMissionService _service;
        ILogger _logger;
        public RequestMissionRepository(RequestMissionService service, ILogger<RequestMissionRepository> logger)
        {
            this._logger = logger;
            this._service = service;
        }

        public async Task<bool> StartMission(int userId, string missionCode, List<string> characterCodes)
        {
            if (!_service.IsValidCharacterCodes(characterCodes))
            {
                return false;
            }

            using (var context = new GameDbContext())
            {
                var user = await context.UserInfos.Where(u => u.Id == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.RequestMissions)
                    .FirstOrDefaultAsync();

                if (!_service.IsValidMissionCode(user.RequestMissions.Select(m => m.MissionCode), missionCode))
                {
                    return false;
                }

                var characters = user.Characters.Where(c => characterCodes.Contains(c.CharacterID)).ToList();
                if (characters.Count != characterCodes.Count)
                {
                    _logger.LogWarning("요청 캐릭터 코드에 해당하는 캐릭터가 존재하지않음");
                    return false;
                }

                var missionSuccess = _service.IsMissionSuccess(missionCode, characters);
                if (!missionSuccess)
                {
                    _logger.LogWarning("미션 조건에 부합하지않음");
                    return false;
                }

                user.RequestMissions.Add(RequestMission.Create(missionCode));
                context.GameEvents.Add(RequestMissionStartEvent.Create(userId, missionCode, characterCodes).CovertToGameEvent());

                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task RequestMissionCompleteCheck(int userId)
        {
            using (var context = new GameDbContext())
            {
                var user = await context.UserInfos.Where(u => u.Id == userId)
                    .Include(u => u.RequestMissions)
                    .FirstOrDefaultAsync();

                var completeMissions = user.RequestMissions.Where(m => _service.IsMissionComplete(m));
                foreach (var mission in completeMissions)
                {
                    var events = _service.ProcessCompleteMission(user, mission.MissionCode);
                    context.GameEvents.AddRange(events.ConvertAll(e => e.CovertToGameEvent()));
                }

                context.RequestMissions.RemoveRange(completeMissions);
                await context.SaveChangesAsync();
            }
        }


    }
}
