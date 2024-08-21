using Microsoft.EntityFrameworkCore;
using Server.Models.Person;

namespace Server.DB
{
    public partial class ExcelDBContext : DbContext
    {
        public ExcelDBContext(DbContextOptions<ExcelDBContext> options)
            : base(options)
        {
        }
        internal virtual DbSet<Person> Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(k => k.Number);
                entity.Property(k => k.Number)
                        .ValueGeneratedNever();

                entity.Property(k => k.Id)
                        .ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
