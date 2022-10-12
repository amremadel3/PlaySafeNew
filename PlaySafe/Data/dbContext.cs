using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaySafe.Models;

namespace PlaySafe.Data
{
    public class dbContext : DbContext
    {
        public dbContext (DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public DbSet<PlaySafe.Models.user> user { get; set; } = default!;
        public DbSet<PlaySafe.Models.matchHistory> matchHistory { get; set; }
        public DbSet<PlaySafe.Models.userType> userType { get; set; }
        public DbSet<PlaySafe.Models.NFC> NFC { get; set; }
        public DbSet<PlaySafe.Models.specials> specials { get; set; }
        public DbSet<PlaySafe.Models.comments> comments { get; set; }
        public DbSet<PlaySafe.Models.player> player { get; set; }
        public DbSet<PlaySafe.Models.userTypePages> userTypePages { get; set; }
        public DbSet<PlaySafe.Models.entry> entry { get; set; }
    }
}
