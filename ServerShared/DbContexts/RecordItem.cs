using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class RecordItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StarLevel { get; set; } = 0;
    }
}
