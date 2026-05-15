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
    }
}
