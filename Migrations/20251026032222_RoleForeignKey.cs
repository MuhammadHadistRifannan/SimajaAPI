using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimajaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RoleForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_role",
                table: "users",
                column: "role");

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_role",
                table: "users",
                column: "role",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_role",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_role",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
