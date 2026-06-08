using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Events
{
    public class ModifiedItemCountInfo
    {
        public string ItemName { get; set; }
        public int BeforeCount { get; set; }
        public int AfterCount { get; set; }
        public string EventVersion { get; set; } = ServerVersion.Version;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
