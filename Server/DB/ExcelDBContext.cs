using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.DB
{
    public partial class ExcelDBContext : DbContext
    {
        public ExcelDBContext(DbContextOptions<ExcelDBContext> options)
            : base(options)
        {
        }
        internal virtual DbSet<ExcelFile> Files { get; set; }
        internal virtual DbSet<Person> Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExcelFile>(entity => entity.HasKey(k => k.Id));

            modelBuilder.Entity<Person>(entity => entity.HasKey(k => k.Id));

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
