﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.DB;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ExcelDBContext))]
    [Migration("20240903103241_Logs")]
    partial class Logs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Server.Models.Log.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ChangesId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("PersonNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChangesId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Server.Models.OldNewValuePair", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OldNewValuePair");
                });

            modelBuilder.Entity("Server.Models.Person.Person", b =>
                {
                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.Property<byte>("Age")
                        .HasColumnType("tinyint");

                    b.Property<DateTimeOffset>("Birthday")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Server.Models.Log.Log", b =>
                {
                    b.HasOne("Server.Models.OldNewValuePair", "Changes")
                        .WithMany()
                        .HasForeignKey("ChangesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
