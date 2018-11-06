using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace digsite.Data
{
    public partial class DigsiteContext : DbContext
    {
        public DigsiteContext()
        {
        }

        public DigsiteContext(DbContextOptions<DigsiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DigState> DigState { get; set; }
        public virtual DbSet<GameState> GameState { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerState> PlayerState { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=rick;password=safepass;database=digsite;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DigState>(entity =>
            {
                entity.ToTable("DigState", "digsite");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("UQ_DigState_PlayerId")
                    .IsUnique();

                entity.Property(e => e.DigStateId).HasColumnType("int(11)");

                entity.Property(e => e.Depth).HasColumnType("int(11)");

                entity.Property(e => e.Fuel).HasColumnType("int(11)");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.DigState)
                    .HasForeignKey<DigState>(d => d.PlayerId)
                    .HasConstraintName("DigState_ibfk_1");
            });

            modelBuilder.Entity<GameState>(entity =>
            {
                entity.ToTable("GameState", "digsite");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("UQ_GameState_PlayerId")
                    .IsUnique();

                entity.Property(e => e.GameStateId).HasColumnType("int(11)");

                entity.Property(e => e.IsDigging).HasColumnType("tinyint(1)");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.GameState)
                    .HasForeignKey<GameState>(d => d.PlayerId)
                    .HasConstraintName("GameState_ibfk_1");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player", "digsite");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlayerState>(entity =>
            {
                entity.ToTable("PlayerState", "digsite");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("UQ_PlayerState_PlayerId")
                    .IsUnique();

                entity.Property(e => e.PlayerStateId).HasColumnType("int(11)");

                entity.Property(e => e.Money).HasColumnType("int(11)");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerState)
                    .HasForeignKey<PlayerState>(d => d.PlayerId)
                    .HasConstraintName("PlayerState_ibfk_1");
            });
        }
    }
}
