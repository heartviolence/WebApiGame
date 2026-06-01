using System.Collections.Concurrent;

namespace SampleWebApi.UserHealthPings
{
    public class UserHealthPing
    {
        //username,expireTime
        public ConcurrentDictionary<string, DateTime> Lives { get; set; } = new();

        public bool IsAlive(string username)
        {
            if (Lives.TryGetValue(username, out _))
            {
                return true;
            }
            return false;
        }

        public void LiveCheck(string username)
        {
            if (Lives.TryGetValue(username, out _))
            {
                return;
            }
            throw new Exception($"username:{username} healthping is dead");
        }

    }
}
