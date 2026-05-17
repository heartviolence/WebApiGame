using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerShared.DbContexts
{
    [Index(nameof(UserId), nameof(GrantItemId), IsUnique = true)]
    public class ReceievedGrantItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GrantItemId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public UserAccountDetail User { get; set; }
    }
}
