using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ncea.Classifier.Microservice.Data.Migrations
{

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class ContentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE public.""SearchPageContentBlocks""
                                    SET ""Value"" = 'Natural assets'
                                    WHERE ""Key"" IN ('SectionTitle') AND ""Value"" = 'Natural asset';

                                    UPDATE public.""SearchPageContentBlocks""
                                    SET ""Value"" = ""Value"" || ' Select all that apply.'
                                    WHERE ""Key"" IN ('SectionIntroduction');");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE public.""SearchPageContentBlocks""
                                    SET ""Value"" = 'Natural asset'
                                    WHERE ""Key"" IN ('SectionTitle') AND ""Value"" = 'Natural assets';

                                    UPDATE public.""SearchPageContentBlocks""
                                    SET ""Value"" = REPLACE(""Value"", ' Select all that apply.','')
                                    WHERE ""Key"" IN ('SectionIntroduction');");
        }
    }
}
