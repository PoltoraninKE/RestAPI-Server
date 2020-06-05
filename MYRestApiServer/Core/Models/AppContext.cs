using Microsoft.EntityFrameworkCore;

namespace MYRestApiServer.Core.Models
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Details> Details { get; set; }

        public AppContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(d => d.HasKey(u => u.ID)); // Getting user by his ID
            modelBuilder.Entity<Details>(d => d.HasKey(i => i.ID)); // Getting details by ID at user with this ID
            modelBuilder.Entity<User>().HasOne(u => u.Details).WithOne(d => d.User).HasForeignKey<Details>(d => d.UserID).OnDelete(DeleteBehavior.Cascade);// Cascading delete data from DB
            modelBuilder.Entity<UserRole>(UR =>
            {
                UR.HasKey(ur => new { ur.UserID, ur.RoleID }); // Finding this role by key UserRole + RoleID
                UR.HasOne(ur => ur.Role).WithMany(rl => rl.UserRoles).HasForeignKey(ur => ur.RoleID).IsRequired(); // Get Role from UserRoles by RoleID
                UR.HasOne(ur => ur.User).WithMany(rl => rl.UserRoles).HasForeignKey(ur => ur.UserID).IsRequired(); // Get User from UserRoles by UserID
            }); // This labmda to get user role :)
        }
    }
}
