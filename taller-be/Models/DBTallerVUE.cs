using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace taller_be.Models
{
    public partial class DBTallerVUE : DbContext
    {
        public DBTallerVUE()
        {
        }

        public DBTallerVUE(DbContextOptions<DBTallerVUE> options)
            : base(options)
        {
        }

        public virtual DbSet<ItemTask> ItemTasks { get; set; } = null!;
        public virtual DbSet<TaskList> TaskLists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local); Database=appsWeb; Integrated Security=False; Persist Security Info=False; User ID=sa; Password=Elena1399.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemTask>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Caduca)
                    .HasColumnType("datetime")
                    .HasColumnName("caduca");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Lista)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("lista");

                entity.Property(e => e.Terminada)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("terminada")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Texto)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("texto");

                entity.Property(e => e.Visible)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("visible")
                    .HasDefaultValueSql("('S')");

                entity.HasOne(d => d.ListaNavigation)
                    .WithMany(p => p.ItemTasks)
                    .HasForeignKey(d => d.Lista)
                    .HasConstraintName("FK_Tasks_Lists");
            });

            modelBuilder.Entity<TaskList>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("color")
                    .HasDefaultValueSql("('#FFFFFF')");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Visible)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("visible")
                    .HasDefaultValueSql("('S')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
