using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;

#nullable disable

namespace Ncea.Classifier.Microservice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddThemeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var migrationAttribute = (MigrationAttribute)this.GetType()
            .GetCustomAttributes(typeof(MigrationAttribute), false)
            .Single();

            migrationBuilder.Sql(File.ReadAllText(string.Format(
                CultureInfo.InvariantCulture,
                "{1}{0}SeedData{0}{2}",
                Path.DirectorySeparatorChar,
                AppContext.BaseDirectory,
                $"{migrationAttribute.Id}.sql")));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
