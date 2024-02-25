using Microsoft.EntityFrameworkCore;
using WallpaperAppBackend.Model;

namespace WallpaperAppBackend.Context
{
    public class WallpaperDBcontext : DbContext
    {
        public WallpaperDBcontext(DbContextOptions<WallpaperDBcontext> options) : base(options)
        {
           
        }
        public DbSet<User> userList { get; set; }
    }
}
