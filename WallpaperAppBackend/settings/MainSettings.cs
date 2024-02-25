using Microsoft.EntityFrameworkCore;
using WallpaperAppBackend.Context;

namespace WallpaperAppBackend.settings
{
    public class MainSettings
    {
        public static void Configuration(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<WallpaperDBcontext>((options)=>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("postgresConnection"));
            });
        }
    }
}
