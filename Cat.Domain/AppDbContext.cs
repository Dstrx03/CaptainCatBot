using System.Data.Entity;
using Cat.Domain.Entities;
using Cat.Domain.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Cat.Domain
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>();
            modelBuilder.Entity<TestEntity>();
        }
    }
}
