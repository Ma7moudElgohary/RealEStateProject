using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEStateProject.Migrations
{
    /// <inheritdoc />
    public partial class eeeeeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_AgentId",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "AgentId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_AgentId",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "AgentId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
