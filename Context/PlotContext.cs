using Plot.Data.Models.User;
using Microsoft.EntityFrameworkCore;


namespace Plot.Context
{
    public class PlotContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public PlotContext(DbContextOptions<PlotContext> options) : base(options)
        {
            
        }

        
    }
}