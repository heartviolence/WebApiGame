using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerShared.DbContexts
{
    
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }        
    }
}
