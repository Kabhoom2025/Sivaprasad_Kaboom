using System.Text.Json;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Data
{
    public class ApplicationDbContext:DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<EventItem> Events { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuthUser> AuthUsers { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<FeatureToggle> FeatureToggles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureJsonStorage<Project>(modelBuilder, x => x.ProjectData);
            ConfigureJsonStorage<Team>(modelBuilder, x => x.TeamData);
            ConfigureJsonStorage<User>(modelBuilder, x => x.UserData);
            ConfigureJsonStorage<TaskItem>(modelBuilder, x => x.TaskData);
            ConfigureJsonStorage<EventItem>(modelBuilder, x => x.EventData);
            ConfigureJsonStorage<Comment>(modelBuilder, x => x.CommentData);
            modelBuilder.Entity<User>()
            .HasOne(u => u.AuthUser)
                .WithMany()
                .HasForeignKey(u => u.AuthUserId);

        }

        private void ConfigureJsonStorage<TEntity>(
            ModelBuilder modelBuilder,
            System.Linq.Expressions.Expression<Func<TEntity, Dictionary<string, object>>> propertySelector
        ) where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .Property(propertySelector)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null)
                );
        }
    }
}
