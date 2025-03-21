using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taller_be.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    precio = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Duracion = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    rol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValueSql: "('user')"),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TaskList",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    color = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValueSql: "('#FFFFFF')"),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    visible = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "('S')"),
                    UsuarioId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskList", x => x.id);
                    table.ForeignKey(
                        name: "FK_TaskList_Servicio_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskList_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskList_ServicioId",
                table: "TaskList",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskList_UsuarioId",
                table: "TaskList",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__AB6E61640368A471",
                table: "Usuarios",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTask");

            migrationBuilder.DropTable(
                name: "TaskList");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
