using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class DevicesAndEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SomfyEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BaseUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CloudUsername = table.Column<string>(type: "TEXT", nullable: false),
                    CloudPasswordHashed = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SomfyEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SomfyEntities_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TuyaEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyaEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TuyaEntities_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SomfyEntities_DeviceId",
                table: "SomfyEntities",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TuyaEntities_DeviceId",
                table: "TuyaEntities",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SomfyEntities");

            migrationBuilder.DropTable(
                name: "TuyaEntities");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
