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

        public virtual DbSet<Buff> Buff { get; set; }
        public virtual DbSet<DigState> DigState { get; set; }
        public virtual DbSet<GameState> GameState { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<ItemSlot> ItemSlot { get; set; }
        public virtual DbSet<Monster> Monster { get; set; }
        public virtual DbSet<NearbyMonster> NearbyMonster { get; set; }
        public virtual DbSet<NearbyMonsterBuff> NearbyMonsterBuff { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerBuff> PlayerBuff { get; set; }
        public virtual DbSet<PlayerItem> PlayerItem { get; set; }
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
            modelBuilder.Entity<Buff>(entity =>
            {
                entity.ToTable("Buff", "digsite");

                entity.Property(e => e.BuffId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<DigState>(entity =>
            {
                entity.ToTable("DigState", "digsite");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("UQ_DigState_PlayerId")
                    .IsUnique();

                entity.Property(e => e.DigStateId).HasColumnType("int(11)");

                entity.Property(e => e.Depth).HasColumnType("int(11)");

                entity.Property(e => e.Fuel).HasColumnType("decimal(10,2)");

                entity.Property(e => e.IsPaused).HasColumnType("tinyint(1)");

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

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item", "digsite");

                entity.HasIndex(e => e.ItemCategoryId)
                    .HasName("ItemTypeId");

                entity.HasIndex(e => e.ItemSlotId)
                    .HasName("ItemSlotId");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.Cooldown).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ItemCategoryId).HasColumnType("int(11)");

                entity.Property(e => e.ItemSlotId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_ibfk_1");

                entity.HasOne(d => d.ItemSlot)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ItemSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_ibfk_2");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.ToTable("ItemCategory", "digsite");

                entity.Property(e => e.ItemTypeId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemSlot>(entity =>
            {
                entity.ToTable("ItemSlot", "digsite");

                entity.Property(e => e.ItemSlotId).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Monster>(entity =>
            {
                entity.ToTable("Monster", "digsite");

                entity.Property(e => e.MonsterId).HasColumnType("int(11)");

                entity.Property(e => e.Accuracy).HasColumnType("int(11)");

                entity.Property(e => e.Attack).HasColumnType("int(11)");

                entity.Property(e => e.AttackRate).HasColumnType("int(11)");

                entity.Property(e => e.Health).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NearbyMonster>(entity =>
            {
                entity.ToTable("NearbyMonster", "digsite");

                entity.HasIndex(e => e.DigStateId)
                    .HasName("DigStateId");

                entity.HasIndex(e => e.MonsterId)
                    .HasName("MonsterId");

                entity.Property(e => e.NearbyMonsterId).HasColumnType("int(11)");

                entity.Property(e => e.CurrentAttackCooldown)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CurrentHealth).HasColumnType("int(11)");

                entity.Property(e => e.DigStateId).HasColumnType("int(11)");

                entity.Property(e => e.MonsterId).HasColumnType("int(11)");

                entity.HasOne(d => d.DigState)
                    .WithMany(p => p.NearbyMonster)
                    .HasForeignKey(d => d.DigStateId)
                    .HasConstraintName("NearbyMonster_ibfk_2");

                entity.HasOne(d => d.Monster)
                    .WithMany(p => p.NearbyMonster)
                    .HasForeignKey(d => d.MonsterId)
                    .HasConstraintName("NearbyMonster_ibfk_1");
            });

            modelBuilder.Entity<NearbyMonsterBuff>(entity =>
            {
                entity.ToTable("NearbyMonsterBuff", "digsite");

                entity.HasIndex(e => e.BuffId)
                    .HasName("BuffId");

                entity.HasIndex(e => e.NearbyMonsterId)
                    .HasName("NearbyMonsterId");

                entity.Property(e => e.NearbyMonsterBuffId).HasColumnType("int(11)");

                entity.Property(e => e.BuffId).HasColumnType("int(11)");

                entity.Property(e => e.NearbyMonsterId).HasColumnType("int(11)");

                entity.Property(e => e.RemainingDuration).HasColumnType("int(11)");

                entity.HasOne(d => d.Buff)
                    .WithMany(p => p.NearbyMonsterBuff)
                    .HasForeignKey(d => d.BuffId)
                    .HasConstraintName("NearbyMonsterBuff_ibfk_2");

                entity.HasOne(d => d.NearbyMonster)
                    .WithMany(p => p.NearbyMonsterBuff)
                    .HasForeignKey(d => d.NearbyMonsterId)
                    .HasConstraintName("NearbyMonsterBuff_ibfk_1");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player", "digsite");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlayerBuff>(entity =>
            {
                entity.ToTable("PlayerBuff", "digsite");

                entity.HasIndex(e => e.BuffId)
                    .HasName("BuffId");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("PlayerId");

                entity.Property(e => e.PlayerBuffId).HasColumnType("int(11)");

                entity.Property(e => e.BuffId).HasColumnType("int(11)");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.Property(e => e.RemainingDuration).HasColumnType("int(11)");

                entity.HasOne(d => d.Buff)
                    .WithMany(p => p.PlayerBuff)
                    .HasForeignKey(d => d.BuffId)
                    .HasConstraintName("PlayerBuff_ibfk_2");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerBuff)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("PlayerBuff_ibfk_1");
            });

            modelBuilder.Entity<PlayerItem>(entity =>
            {
                entity.ToTable("PlayerItem", "digsite");

                entity.HasIndex(e => e.ItemId)
                    .HasName("ItemId");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("PlayerId");

                entity.Property(e => e.PlayerItemId).HasColumnType("int(11)");

                entity.Property(e => e.CurrentCooldown)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsEquipped)
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ItemId).HasColumnType("int(11)");

                entity.Property(e => e.PlayerId).HasColumnType("int(11)");

                entity.Property(e => e.UpgradeLevel).HasColumnType("int(11)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.PlayerItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlayerItem_ibfk_2");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerItem)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlayerItem_ibfk_1");
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
