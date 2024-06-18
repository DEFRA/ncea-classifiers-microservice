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
    [Migration("20240618134925_AddThemeData")]
    partial class AddThemeData
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
                        .HasColumnOrder(5);

                    b.Property<string>("Definition")
                        .IsRequired()
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

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.CategorySubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(4);

                    b.Property<string>("SubCategoryCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(5);

                    b.HasKey("Id");

                    b.HasIndex("CategoryCode");

                    b.HasIndex("SubCategoryCode");

                    b.ToTable("CategorySubCategory");
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

                    b.Property<string>("SectionIntroduction")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnOrder(3);

                    b.Property<string>("SectionTitle")
                        .IsRequired()
                        .HasColumnType("text")
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

                    b.HasKey("Id");

                    b.HasIndex("ThemeCode");

                    b.ToTable("SearchPageContentBlocks");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SubCategory", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(5);

                    b.Property<string>("Definition")
                        .IsRequired()
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
                        .IsRequired()
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

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.ThemeCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(4);

                    b.Property<string>("ThemeCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(5);

                    b.HasKey("Id");

                    b.HasIndex("CategoryCode");

                    b.HasIndex("ThemeCode");

                    b.ToTable("ThemeCategory");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.CategorySubCategory", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.SubCategory", null)
                        .WithMany()
                        .HasForeignKey("SubCategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.SearchPageContent", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Theme", "Theme")
                        .WithMany("SearchPageContentBlocks")
                        .HasForeignKey("ThemeCode");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.ThemeCategory", b =>
                {
                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ncea.Classifier.Microservice.Data.Entities.Theme", null)
                        .WithMany()
                        .HasForeignKey("ThemeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ncea.Classifier.Microservice.Data.Entities.Theme", b =>
                {
                    b.Navigation("SearchPageContentBlocks");
                });
#pragma warning restore 612, 618
        }
    }
}
