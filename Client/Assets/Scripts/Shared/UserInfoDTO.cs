
using System.Collections.Generic;

namespace Assets.Scripts.Shared
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Nickname { get; set; }

        public List<GameCharacterDTO> Characters { get; set; } = new();

        public int Crystal { get; set; }

        public List<RequestMissionDTO> RequestMissions { get; set; } = new();
    }
}
