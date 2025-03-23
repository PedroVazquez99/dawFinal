﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using taller_be.Models;

#nullable disable

namespace taller_be.Migrations
{
    [DbContext(typeof(PeluqueriaBDDContext))]
    [Migration("20250321230506_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("taller_be.Models.ItemTask", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(18,0)")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime?>("Caduca")
                        .HasColumnType("datetime")
                        .HasColumnName("caduca");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha");

                    b.Property<decimal>("Lista")
                        .HasColumnType("numeric(18,0)")
                        .HasColumnName("lista");

                    b.Property<string>("Terminada")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .HasColumnName("terminada")
                        .HasDefaultValueSql("('N')")
                        .IsFixedLength();

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("texto");

                    b.Property<string>("Visible")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .HasColumnName("visible")
                        .HasDefaultValueSql("('S')")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.ToTable("ItemTask", (string)null);
                });

            modelBuilder.Entity("taller_be.Models.Servicio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("descripcion");

                    b.Property<TimeSpan>("Duracion")
                        .HasColumnType("time");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("nombre");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("precio");

                    b.HasKey("Id");

                    b.ToTable("Servicio");
                });

            modelBuilder.Entity("taller_be.Models.TaskList", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(18,0)")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("color")
                        .HasDefaultValueSql("('#FFFFFF')");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("nombre");

                    b.Property<int>("ServicioId")
                        .HasColumnType("int");

                    b.Property<decimal>("UsuarioId")
                        .HasColumnType("numeric(18,0)");

                    b.Property<string>("Visible")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .HasColumnName("visible")
                        .HasDefaultValueSql("('S')")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("ServicioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TaskList", (string)null);
                });

            modelBuilder.Entity("taller_be.Models.Usuario", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(18,0)")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("email");

                    b.Property<DateTime>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_registro")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("nombre");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("password_hash");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("rol")
                        .HasDefaultValueSql("('user')");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "UQ__Usuarios__AB6E61640368A471")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("taller_be.Models.TaskList", b =>
                {
                    b.HasOne("taller_be.Models.Servicio", "Servicio")
                        .WithMany()
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taller_be.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Servicio");

                    b.Navigation("Usuario");
                });
#pragma warning restore 612, 618
        }
    }
}
