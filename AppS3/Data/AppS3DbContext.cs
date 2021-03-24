using AppS3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Data
{
    public class AppS3DbContext : IdentityDbContext<User>
    {
        public AppS3DbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
