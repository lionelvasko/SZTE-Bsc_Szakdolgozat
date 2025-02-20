using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class SomfyDevicesUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Email");

            migrationBuilder.CreateTable(
                name: "Connectivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ProtocolVersion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connectivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Definition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UiProfiles = table.Column<string>(type: "TEXT", nullable: false),
                    WidgetName = table.Column<string>(type: "TEXT", nullable: false),
                    UiClass = table.Column<string>(type: "TEXT", nullable: false),
                    QualifiedName = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GatewayId = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    SubType = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoUpdateEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeReliable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConnectivityId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpToDate = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdateStatus = table.Column<string>(type: "TEXT", nullable: false),
                    SyncInProgress = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdateCriticityLevel = table.Column<string>(type: "TEXT", nullable: false),
                    AutomaticUpdate = table.Column<bool>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false),
                    Functions = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_Connectivity_ConnectivityId",
                        column: x => x.ConnectivityId,
                        principalTable: "Connectivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Device_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandName = table.Column<string>(type: "TEXT", nullable: false),
                    Nparams = table.Column<int>(type: "INTEGER", nullable: false),
                    DefinitionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Command_Definition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceURL = table.Column<string>(type: "TEXT", nullable: false),
                    Available = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ControllableName = table.Column<string>(type: "TEXT", nullable: false),
                    DefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdateTime = table.Column<long>(type: "INTEGER", nullable: false),
                    Widget = table.Column<string>(type: "TEXT", nullable: false),
                    UiClass = table.Column<string>(type: "TEXT", nullable: false),
                    PlaceOID = table.Column<string>(type: "TEXT", nullable: false),
                    OID = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity_Definition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    DefinitionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Definition_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_EntityId",
                table: "Attribute",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Command_DefinitionId",
                table: "Command",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_ConnectivityId",
                table: "Device",
                column: "ConnectivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_UserId",
                table: "Device",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_DefinitionId",
                table: "Entity",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_UserId",
                table: "Entity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_State_DefinitionId",
                table: "State",
                column: "DefinitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Entity");

            migrationBuilder.DropTable(
                name: "Connectivity");

            migrationBuilder.DropTable(
                name: "Definition");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Username");
        }
    }
}
