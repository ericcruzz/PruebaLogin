using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaLogin.Models;

public partial class StagingContext : DbContext
{
    public StagingContext(DbContextOptions<StagingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Password__1788CCACD19391F8");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Password)
                .HasForeignKey<Password>(d => d.UserId)
                .HasConstraintName("FK_Users_Passwords");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6CD291A1");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RecordDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
