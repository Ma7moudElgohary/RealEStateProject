using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEStateProject.Migrations
{
    /// <inheritdoc />
    public partial class lEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRequests_AspNetUsers_UserId",
                table: "PropertyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRequests_Properties_PropertyId",
                table: "PropertyRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyRequests",
                table: "PropertyRequests");

            migrationBuilder.RenameTable(
                name: "PropertyRequests",
                newName: "PropertyRequest");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRequests_UserId",
                table: "PropertyRequest",
                newName: "IX_PropertyRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRequests_PropertyId",
                table: "PropertyRequest",
                newName: "IX_PropertyRequest_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyRequest",
                table: "PropertyRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRequest_AspNetUsers_UserId",
                table: "PropertyRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRequest_Properties_PropertyId",
                table: "PropertyRequest",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRequest_AspNetUsers_UserId",
                table: "PropertyRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRequest_Properties_PropertyId",
                table: "PropertyRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyRequest",
                table: "PropertyRequest");

            migrationBuilder.RenameTable(
                name: "PropertyRequest",
                newName: "PropertyRequests");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRequest_UserId",
                table: "PropertyRequests",
                newName: "IX_PropertyRequests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyRequest_PropertyId",
                table: "PropertyRequests",
                newName: "IX_PropertyRequests_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyRequests",
                table: "PropertyRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRequests_AspNetUsers_UserId",
                table: "PropertyRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRequests_Properties_PropertyId",
                table: "PropertyRequests",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
