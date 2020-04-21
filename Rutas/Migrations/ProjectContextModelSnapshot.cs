using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Rutas.Models;

namespace Rutas.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Rutas.Models.Almacenes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apocope");

                    b.Property<string>("Descripcion");

                    b.HasKey("Id");

                    b.ToTable("Almacenes");
                });

            modelBuilder.Entity("Rutas.Models.Inventario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlmacenesId");

                    b.Property<int>("Balance");

                    b.Property<int>("LocacionesId");

                    b.Property<int>("PartNumberId");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenesId");

                    b.HasIndex("LocacionesId");

                    b.HasIndex("PartNumberId");

                    b.ToTable("Inventario");
                });

            modelBuilder.Entity("Rutas.Models.Locaciones", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apocope");

                    b.Property<string>("Descripcion");

                    b.HasKey("Id");

                    b.ToTable("Locaciones");
                });

            modelBuilder.Entity("Rutas.Models.Localidades", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Costo");

                    b.Property<string>("Departamento");

                    b.Property<string>("Distrito");

                    b.Property<double>("Latitud");

                    b.Property<string>("Localidad");

                    b.Property<double>("Longitud");

                    b.Property<string>("Provincia");

                    b.Property<int>("Proyectoid");

                    b.Property<int>("Serviciosid");

                    b.Property<string>("Telefonos");

                    b.Property<int>("Vsatid");

                    b.HasKey("Id");

                    b.HasIndex("Proyectoid");

                    b.HasIndex("Serviciosid");

                    b.ToTable("Localidades");
                });

            modelBuilder.Entity("Rutas.Models.PartNumbers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Categoria");

                    b.Property<string>("Descripcion");

                    b.Property<string>("PartNumber");

                    b.HasKey("Id");

                    b.ToTable("PartNumbers");
                });

            modelBuilder.Entity("Rutas.Models.Proyectos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion");

                    b.HasKey("Id");

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("Rutas.Models.Rutas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<int>("Estado");

                    b.Property<DateTime>("FechaFin");

                    b.Property<DateTime>("FechaInicio");

                    b.Property<int>("TecnicosId");

                    b.HasKey("Id");

                    b.HasIndex("TecnicosId");

                    b.ToTable("Rutas");
                });

            modelBuilder.Entity("Rutas.Models.SelectedInventario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlmacenesId");

                    b.Property<int>("Balance");

                    b.Property<int>("LocacionesId");

                    b.Property<int>("PartNumberId");

                    b.Property<int>("Rutasid");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenesId");

                    b.HasIndex("LocacionesId");

                    b.HasIndex("PartNumberId");

                    b.HasIndex("Rutasid");

                    b.ToTable("SelectedInventario");
                });

            modelBuilder.Entity("Rutas.Models.SelectedLocalidades", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<double>("Costo");

                    b.Property<string>("Departamento");

                    b.Property<string>("Distrito");

                    b.Property<int>("Estado");

                    b.Property<double>("Latitud");

                    b.Property<string>("Localidad");

                    b.Property<double>("Longitud");

                    b.Property<string>("Provincia");

                    b.Property<int>("Proyectoid");

                    b.Property<int>("Rutasid");

                    b.Property<int>("Serviciosid");

                    b.Property<string>("Telefonos");

                    b.Property<int>("Vsatid");

                    b.HasKey("Id");

                    b.HasIndex("Proyectoid");

                    b.HasIndex("Rutasid");

                    b.HasIndex("Serviciosid");

                    b.ToTable("SelectedLocalidades");
                });

            modelBuilder.Entity("Rutas.Models.Servicios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion");

                    b.HasKey("Id");

                    b.ToTable("Servicios");
                });

            modelBuilder.Entity("Rutas.Models.Tecnicos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apellido");

                    b.Property<string>("Apocope");

                    b.Property<string>("Direccion");

                    b.Property<string>("Dni");

                    b.Property<string>("Email");

                    b.Property<DateTime>("FechaNacimiento");

                    b.Property<string>("Nombre");

                    b.Property<int>("Telefono");

                    b.HasKey("Id");

                    b.ToTable("Tecnicos");
                });

            modelBuilder.Entity("Rutas.Models.Inventario", b =>
                {
                    b.HasOne("Rutas.Models.Almacenes", "Almacenes")
                        .WithMany("Inventario")
                        .HasForeignKey("AlmacenesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Locaciones", "Locaciones")
                        .WithMany("Inventario")
                        .HasForeignKey("LocacionesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.PartNumbers", "PartNumbers")
                        .WithMany("Inventario")
                        .HasForeignKey("PartNumberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rutas.Models.Localidades", b =>
                {
                    b.HasOne("Rutas.Models.Proyectos", "Proyectos")
                        .WithMany("Localidades")
                        .HasForeignKey("Proyectoid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Servicios", "Servicios")
                        .WithMany("Localidades")
                        .HasForeignKey("Serviciosid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rutas.Models.Rutas", b =>
                {
                    b.HasOne("Rutas.Models.Tecnicos", "Tecnicos")
                        .WithMany("Rutas")
                        .HasForeignKey("TecnicosId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rutas.Models.SelectedInventario", b =>
                {
                    b.HasOne("Rutas.Models.Almacenes", "Almacenes")
                        .WithMany("SelectedInventario")
                        .HasForeignKey("AlmacenesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Locaciones", "Locaciones")
                        .WithMany("SelectedInventario")
                        .HasForeignKey("LocacionesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.PartNumbers", "PartNumbers")
                        .WithMany("SelectedInventario")
                        .HasForeignKey("PartNumberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Rutas", "Rutas")
                        .WithMany("SelectedInventario")
                        .HasForeignKey("Rutasid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Rutas.Models.SelectedLocalidades", b =>
                {
                    b.HasOne("Rutas.Models.Proyectos", "Proyectos")
                        .WithMany("SelectedLocalidades")
                        .HasForeignKey("Proyectoid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Rutas", "Rutas")
                        .WithMany("SelectedLocalidades")
                        .HasForeignKey("Rutasid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Rutas.Models.Servicios", "Servicios")
                        .WithMany("SelectedLocalidades")
                        .HasForeignKey("Serviciosid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
