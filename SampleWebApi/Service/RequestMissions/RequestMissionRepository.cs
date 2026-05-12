using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Shards;

namespace SampleWebApi.Service.RequestMissions
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

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = await context.UserDetails.Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.RequestMissions.Where(m => m.MissionCode == missionCode))
                    .FirstOrDefaultAsync();

                if (user.RequestMissions.Count > 0)
                {
                    _logger.LogWarning("이미 존재하는 의뢰 미션코드,missionCode:{MissionCode}", missionCode);
                    return false;
                }

                if (!_service.IsValidMissionCode(missionCode))
                {
                    return false;
                }

                var characters = user.Characters.Where(c => characterCodes.Contains(c.Name)).ToList();
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

                user.RowVersion = Guid.NewGuid();
                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task RequestMissionCompleteCheck(int userId)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = await context.UserDetails.Where(u => u.UserId == userId)
                    .Include(u => u.RequestMissions)
                    .FirstOrDefaultAsync();

                var completeMissions = user.RequestMissions.Where(m => _service.IsMissionComplete(m));
                foreach (var mission in completeMissions)
                {
                    var events = _service.ProcessCompleteMission(user, mission.MissionCode);
                    context.GameEvents.AddRange(events.ConvertAll(e => e.CovertToGameEvent()));
                }

                context.RequestMissions.RemoveRange(completeMissions);
                user.RowVersion = Guid.NewGuid();
                await context.SaveChangesAsync();
            }
        }


    }
}
