using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Collection.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<CollectionDb> CollectionDbs { get; set; }

        public DbSet<Item> Items { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
