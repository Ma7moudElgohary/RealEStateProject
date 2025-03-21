using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEStateProject.Migrations
{
    /// <inheritdoc />
    public partial class editt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "PropertyImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "PropertyImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
