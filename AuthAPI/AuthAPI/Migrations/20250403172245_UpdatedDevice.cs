using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Device_DeviceId",
                table: "Entities");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Entities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Device_DeviceId",
                table: "Entities",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Device_DeviceId",
                table: "Entities");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Entities",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Device_DeviceId",
                table: "Entities",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");
        }
    }
}
