using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyDB.Migrations
{
    /// <inheritdoc />
    public partial class newModelforCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotPoints_Calendar_CalendarId",
                table: "PlotPoints");

            migrationBuilder.DropTable(
                name: "PlotPointsCharacterRelationships");

            migrationBuilder.DropTable(
                name: "PlotPointsCharacters");

            migrationBuilder.DropTable(
                name: "PlotPointsEras");

            migrationBuilder.DropTable(
                name: "PlotPointsEvents");

            migrationBuilder.DropTable(
                name: "PlotPointsFactions");

            migrationBuilder.DropTable(
                name: "PlotPointsItems");

            migrationBuilder.DropTable(
                name: "PlotPointsLocations");

            migrationBuilder.RenameColumn(
                name: "CalendarId",
                table: "PlotPoints",
                newName: "startDateId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPoints_CalendarId",
                table: "PlotPoints",
                newName: "IX_PlotPoints_startDateId");

            migrationBuilder.AddColumn<int>(
                name: "endDateId",
                table: "PlotPoints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_endDateId",
                table: "PlotPoints",
                column: "endDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPoints_Calendar_endDateId",
                table: "PlotPoints",
                column: "endDateId",
                principalTable: "Calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPoints_Calendar_startDateId",
                table: "PlotPoints",
                column: "startDateId",
                principalTable: "Calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotPoints_Calendar_endDateId",
                table: "PlotPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPoints_Calendar_startDateId",
                table: "PlotPoints");

            migrationBuilder.DropIndex(
                name: "IX_PlotPoints_endDateId",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "endDateId",
                table: "PlotPoints");

            migrationBuilder.RenameColumn(
                name: "startDateId",
                table: "PlotPoints",
                newName: "CalendarId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPoints_startDateId",
                table: "PlotPoints",
                newName: "IX_PlotPoints_CalendarId");

            migrationBuilder.CreateTable(
                name: "PlotPointsCharacterRelationships",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    CharacterRelationshipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsCharacterRelationships", x => new { x.PlotPointId, x.CharacterRelationshipId });
                    table.ForeignKey(
                        name: "FK_PlotPointsCharacterRelationships_CharacterRelationships_CharacterRelationshipId",
                        column: x => x.CharacterRelationshipId,
                        principalTable: "CharacterRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsCharacterRelationships_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsCharacters",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsCharacters", x => new { x.PlotPointId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_PlotPointsCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsCharacters_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsEras",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    EraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsEras", x => new { x.PlotPointId, x.EraId });
                    table.ForeignKey(
                        name: "FK_PlotPointsEras_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsEras_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsEvents",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsEvents", x => new { x.PlotPointId, x.EventId });
                    table.ForeignKey(
                        name: "FK_PlotPointsEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsEvents_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsFactions",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsFactions", x => new { x.PlotPointId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_PlotPointsFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsFactions_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsItems",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsItems", x => new { x.PlotPointId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_PlotPointsItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsItems_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsLocations",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsLocations", x => new { x.PlotPointId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_PlotPointsLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsLocations_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsCharacterRelationships_CharacterRelationshipId",
                table: "PlotPointsCharacterRelationships",
                column: "CharacterRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsCharacters_CharacterId",
                table: "PlotPointsCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsEras_EraId",
                table: "PlotPointsEras",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsEvents_EventId",
                table: "PlotPointsEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsFactions_FactionId",
                table: "PlotPointsFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsItems_ItemId",
                table: "PlotPointsItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsLocations_LocationId",
                table: "PlotPointsLocations",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPoints_Calendar_CalendarId",
                table: "PlotPoints",
                column: "CalendarId",
                principalTable: "Calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
