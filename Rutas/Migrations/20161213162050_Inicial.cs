using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rutas.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Almacenes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Apocope = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almacenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locaciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Apocope = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Categoria = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    PartNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Apellido = table.Column<string>(nullable: true),
                    Apocope = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    Dni = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Telefono = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AlmacenesId = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    LocacionesId = table.Column<int>(nullable: false),
                    PartNumberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventario_Almacenes_AlmacenesId",
                        column: x => x.AlmacenesId,
                        principalTable: "Almacenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventario_Locaciones_LocacionesId",
                        column: x => x.LocacionesId,
                        principalTable: "Locaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventario_PartNumbers_PartNumberId",
                        column: x => x.PartNumberId,
                        principalTable: "PartNumbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Costo = table.Column<double>(nullable: false),
                    Departamento = table.Column<string>(nullable: true),
                    Distrito = table.Column<string>(nullable: true),
                    Latitud = table.Column<double>(nullable: false),
                    Localidad = table.Column<string>(nullable: true),
                    Longitud = table.Column<double>(nullable: false),
                    Provincia = table.Column<string>(nullable: true),
                    Proyectoid = table.Column<int>(nullable: false),
                    Serviciosid = table.Column<int>(nullable: false),
                    Telefonos = table.Column<string>(nullable: true),
                    Vsatid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localidades_Proyectos_Proyectoid",
                        column: x => x.Proyectoid,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Localidades_Servicios_Serviciosid",
                        column: x => x.Serviciosid,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rutas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Color = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    FechaFin = table.Column<DateTime>(nullable: false),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    TecnicosId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rutas_Tecnicos_TecnicosId",
                        column: x => x.TecnicosId,
                        principalTable: "Tecnicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedInventario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AlmacenesId = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    LocacionesId = table.Column<int>(nullable: false),
                    PartNumberId = table.Column<int>(nullable: false),
                    Rutasid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedInventario_Almacenes_AlmacenesId",
                        column: x => x.AlmacenesId,
                        principalTable: "Almacenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedInventario_Locaciones_LocacionesId",
                        column: x => x.LocacionesId,
                        principalTable: "Locaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedInventario_PartNumbers_PartNumberId",
                        column: x => x.PartNumberId,
                        principalTable: "PartNumbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedInventario_Rutas_Rutasid",
                        column: x => x.Rutasid,
                        principalTable: "Rutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedLocalidades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Color = table.Column<string>(nullable: true),
                    Costo = table.Column<double>(nullable: false),
                    Departamento = table.Column<string>(nullable: true),
                    Distrito = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    Latitud = table.Column<double>(nullable: false),
                    Localidad = table.Column<string>(nullable: true),
                    Longitud = table.Column<double>(nullable: false),
                    Provincia = table.Column<string>(nullable: true),
                    Proyectoid = table.Column<int>(nullable: false),
                    Rutasid = table.Column<int>(nullable: false),
                    Serviciosid = table.Column<int>(nullable: false),
                    Telefonos = table.Column<string>(nullable: true),
                    Vsatid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedLocalidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedLocalidades_Proyectos_Proyectoid",
                        column: x => x.Proyectoid,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedLocalidades_Rutas_Rutasid",
                        column: x => x.Rutasid,
                        principalTable: "Rutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedLocalidades_Servicios_Serviciosid",
                        column: x => x.Serviciosid,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_AlmacenesId",
                table: "Inventario",
                column: "AlmacenesId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_LocacionesId",
                table: "Inventario",
                column: "LocacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_PartNumberId",
                table: "Inventario",
                column: "PartNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_Proyectoid",
                table: "Localidades",
                column: "Proyectoid");

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_Serviciosid",
                table: "Localidades",
                column: "Serviciosid");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_TecnicosId",
                table: "Rutas",
                column: "TecnicosId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedInventario_AlmacenesId",
                table: "SelectedInventario",
                column: "AlmacenesId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedInventario_LocacionesId",
                table: "SelectedInventario",
                column: "LocacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedInventario_PartNumberId",
                table: "SelectedInventario",
                column: "PartNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedInventario_Rutasid",
                table: "SelectedInventario",
                column: "Rutasid");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedLocalidades_Proyectoid",
                table: "SelectedLocalidades",
                column: "Proyectoid");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedLocalidades_Rutasid",
                table: "SelectedLocalidades",
                column: "Rutasid");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedLocalidades_Serviciosid",
                table: "SelectedLocalidades",
                column: "Serviciosid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Localidades");

            migrationBuilder.DropTable(
                name: "SelectedInventario");

            migrationBuilder.DropTable(
                name: "SelectedLocalidades");

            migrationBuilder.DropTable(
                name: "Almacenes");

            migrationBuilder.DropTable(
                name: "Locaciones");

            migrationBuilder.DropTable(
                name: "PartNumbers");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Rutas");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Tecnicos");
        }
    }
}
