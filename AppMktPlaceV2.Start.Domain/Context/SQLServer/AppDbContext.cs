#region IMPORT
using Microsoft.EntityFrameworkCore;
using AppMktPlaceV2.Start.Domain.Entities;
using AppMktPlaceV2.Start.Domain.Entities;
#endregion IMPORT

namespace AppMktPlaceV2.Start.Domain.Context.SQLServer
{
    public partial class AppDbContext : DbContext
    {
        #region CONTRUTORES
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        #endregion CONTRUTORES

        #region DBCONTEXT
        public DbSet<Trade> Trades { get; set; }
        #endregion DBCONTEXT

        #region METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>(entity =>
            {
                entity.HasKey(e => e.TradeId);

                entity.Property(e => e.DateRegistered);

                entity.Property(e => e.DateUpdated);

                entity.Property(e => e. ClientSector)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientRisk)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #endregion METHODS
    }
}
