using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Revision.Migrations
{
    /// <inheritdoc />
    public partial class newUpDate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Players",
                newName: "BirthOfDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthOfDate",
                table: "Players",
                newName: "MyProperty");
        }
    }
}
