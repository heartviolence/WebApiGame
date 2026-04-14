using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServerShared.DbContexts
{
    [Index(nameof(UserId), nameof(GrantItemId), IsUnique = true)]
    public class ReceievedGrantItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GrantItemId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserAccountDetail User { get; set; }
    }
}
