using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Shared
{
    public class RequestMissionStartRequest
    {
        public string MissionCode { get; set; }
        public List<string> CharacterCode { get; set; } = new();
    }
}
