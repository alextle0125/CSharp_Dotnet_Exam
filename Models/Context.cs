using Microsoft.EntityFrameworkCore;
 
namespace ActivityCenter.Models
{
    public class Context : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public Context(DbContextOptions options) : base(options) { }
		public DbSet<User> Users {get;set;}
		public DbSet<ACActivity> ACActivities {get;set;}  
		public DbSet<Join> Joins {get;set;}  
    }
}