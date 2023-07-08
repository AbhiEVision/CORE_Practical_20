using Microsoft.EntityFrameworkCore;
using Practical_20.Models;

namespace Practical_20.Data
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{

		}

		public DbSet<Student> Students { get; set; }

		public DbSet<User> Users { get; set; }
	}
}
