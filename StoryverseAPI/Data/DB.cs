using Microsoft.EntityFrameworkCore;

namespace StoryverseAPI.Data
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Book> Books { get; set; }
        public DbSet<Models.Chapter> Chapters { get; set; }
    }
}
