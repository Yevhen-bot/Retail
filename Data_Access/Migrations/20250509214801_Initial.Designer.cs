﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Data_Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_Access.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250509214801_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Data_Access.Entities.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Area")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.ComplexProperty<Dictionary<string, object>>("Adress", "Data_Access.Entities.Building.Adress#Adress", b1 =>
                        {
                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("varchar(40)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("varchar(40)")
                                .HasColumnName("Country");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasMaxLength(4)
                                .HasColumnType("varchar(4)")
                                .HasColumnName("HouseNumber");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Role", "Data_Access.Entities.Building.Role#BuildingRole", b1 =>
                        {
                            b1.Property<string>("RoleName")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)")
                                .HasColumnName("Role");
                        });

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("Data_Access.Entities.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BuildingId")
                        .HasColumnType("int");

                    b.ComplexProperty<Dictionary<string, object>>("Age", "Data_Access.Entities.Worker.Age#Age", b1 =>
                        {
                            b1.Property<DateOnly>("BirhtDate")
                                .HasColumnType("date")
                                .HasColumnName("BirthDate");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ExaustionLevel", "Data_Access.Entities.Worker.ExaustionLevel#ExhaustionLevel", b1 =>
                        {
                            b1.Property<double>("Level")
                                .HasPrecision(18, 2)
                                .HasColumnType("double")
                                .HasColumnName("ExaustionLevel");

                            b1.Property<double>("Progression")
                                .HasColumnType("double");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HomeAdress", "Data_Access.Entities.Worker.HomeAdress#Adress", b1 =>
                        {
                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("varchar(40)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("varchar(40)")
                                .HasColumnName("Country");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasMaxLength(4)
                                .HasColumnType("varchar(4)")
                                .HasColumnName("HouseNumber");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "Data_Access.Entities.Worker.Name#Name", b1 =>
                        {
                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)")
                                .HasColumnName("LastName");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Role", "Data_Access.Entities.Worker.Role#Role", b1 =>
                        {
                            b1.Property<string>("RoleName")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)")
                                .HasColumnName("Role");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Salary", "Data_Access.Entities.Worker.Salary#Salary", b1 =>
                        {
                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Salary");
                        });

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Data_Access.Entities.Worker", b =>
                {
                    b.HasOne("Data_Access.Entities.Building", "Building")
                        .WithMany("Workers")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("Data_Access.Entities.Building", b =>
                {
                    b.Navigation("Workers");
                });
#pragma warning restore 612, 618
        }
    }
}
