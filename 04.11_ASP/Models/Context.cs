using Microsoft.EntityFrameworkCore;

namespace _04._11_ASP.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<UserModel> UserModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().Ignore(p => p.AvatarImg);
        }
    }
}
