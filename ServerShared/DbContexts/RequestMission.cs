namespace ServerShared.DbContexts
{
    public class RequestMission
    {
        public int Id { get; set; }
        public string MissionCode { get; set; }

        public DateTime StartTime { get; set; }

        public static RequestMission Create(string missionCode)
        {
            return new RequestMission
            {
                MissionCode = missionCode,
                StartTime = DateTime.Now,
            };
        }
    }
}
