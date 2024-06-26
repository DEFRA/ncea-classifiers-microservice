﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ncea.Classifier.Microservice.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ncea.Classifier.Microservice.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240620181826_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Category", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(6);

                    b.Property<string>("Definition")
                        .HasColumnType("text")
                        .HasColumnOrder(4);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnOrder(3);

                    b.Property<string>("ThemeCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(5);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(7);

                    b.HasKey("Code");

                    b.HasIndex("ThemeCode");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SearchPageContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(6);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnOrder(2);

                    b.Property<int>("Step")
                        .HasColumnType("integer")
                        .HasColumnOrder(5);

                    b.Property<string>("ThemeCode")
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(4);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(7);

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnOrder(3);

                    b.HasKey("Id");

                    b.HasIndex("ThemeCode");

                    b.ToTable("SearchPageContentBlocks");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SubCategory", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<string>("CategoryCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(5);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(6);

                    b.Property<string>("Definition")
                        .HasColumnType("text")
                        .HasColumnOrder(4);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(7);

                    b.HasKey("Code");

                    b.HasIndex("CategoryCode");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Theme", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(5);

                    b.Property<string>("Definition")
                        .HasColumnType("text")
                        .HasColumnOrder(4);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(6);

                    b.HasKey("Code");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Category", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Theme", "Theme")
                        .WithMany("Categories")
                        .HasForeignKey("ThemeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SearchPageContent", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Theme", "Theme")
                        .WithMany("SearchPageContentBlocks")
                        .HasForeignKey("ThemeCode");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SubCategory", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Theme", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("SearchPageContentBlocks");
                });
#pragma warning restore 612, 618
        }
    }
}
