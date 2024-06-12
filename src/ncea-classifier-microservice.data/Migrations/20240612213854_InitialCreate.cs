using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ncea.Classifier.Microservice.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "varchar(10)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "varchar(10)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "varchar(10)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "CategorySubCategory",
                columns: table => new
                {
                    CategoryCode = table.Column<string>(type: "varchar(10)", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySubCategory", x => new { x.CategoryCode, x.SubCategoryCode });
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_Categories_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "Categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_SubCategories_SubCategoryCode",
                        column: x => x.SubCategoryCode,
                        principalTable: "SubCategories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchPageContentBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionTitle = table.Column<string>(type: "text", nullable: false),
                    SectionIntroduction = table.Column<string>(type: "text", nullable: false),
                    ThemeCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchPageContentBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchPageContentBlocks_Themes_ThemeCode",
                        column: x => x.ThemeCode,
                        principalTable: "Themes",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "ThemeCategory",
                columns: table => new
                {
                    ThemeCode = table.Column<string>(type: "varchar(10)", nullable: false),
                    CategoryCode = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeCategory", x => new { x.CategoryCode, x.ThemeCode });
                    table.ForeignKey(
                        name: "FK_ThemeCategory_Categories_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "Categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThemeCategory_Themes_ThemeCode",
                        column: x => x.ThemeCode,
                        principalTable: "Themes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategory_SubCategoryCode",
                table: "CategorySubCategory",
                column: "SubCategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_SearchPageContentBlocks_ThemeCode",
                table: "SearchPageContentBlocks",
                column: "ThemeCode");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeCategory_ThemeCode",
                table: "ThemeCategory",
                column: "ThemeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySubCategory");

            migrationBuilder.DropTable(
                name: "SearchPageContentBlocks");

            migrationBuilder.DropTable(
                name: "ThemeCategory");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
