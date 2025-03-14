using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDevicesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SomfyDevices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    GatewayId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Creation_Time = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SomfyDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SomfyDevices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TuyaDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Creation_Time = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyaDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TuyaDevices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    SomfyDeviceId = table.Column<string>(type: "TEXT", nullable: true),
                    TuyaDeviceId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Brightness = table.Column<string>(type: "TEXT", nullable: true),
                    ColorMode = table.Column<string>(type: "TEXT", nullable: true),
                    Online = table.Column<bool>(type: "INTEGER", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    ColorTemp = table.Column<int>(type: "INTEGER", nullable: true),
                    TuyaEntity_UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity_AspNetUsers_TuyaEntity_UserId",
                        column: x => x.TuyaEntity_UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entity_SomfyDevices_SomfyDeviceId",
                        column: x => x.SomfyDeviceId,
                        principalTable: "SomfyDevices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entity_TuyaDevices_TuyaDeviceId",
                        column: x => x.TuyaDeviceId,
                        principalTable: "TuyaDevices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity_SomfyDeviceId",
                table: "Entity",
                column: "SomfyDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_TuyaDeviceId",
                table: "Entity",
                column: "TuyaDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_TuyaEntity_UserId",
                table: "Entity",
                column: "TuyaEntity_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_UserId",
                table: "Entity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SomfyDevices_UserId",
                table: "SomfyDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TuyaDevices_UserId",
                table: "TuyaDevices",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity");

            migrationBuilder.DropTable(
                name: "SomfyDevices");

            migrationBuilder.DropTable(
                name: "TuyaDevices");
        }
    }
}
