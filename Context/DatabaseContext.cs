using Microsoft.EntityFrameworkCore;
using _2FA.Models;
using _2FA.Utilities;
namespace _2FA.Context
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext() { }
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(AppConfiguration.GetValue<string>("ConnectionStrings:DbConnection"));
			base.OnConfiguring(optionsBuilder);
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Code> Codes { get; set; }
	}
}