using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyDB.Migrations
{
    /// <inheritdoc />
    public partial class NewLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Crown = table.Column<int>(type: "int", nullable: true),
                    Shilling = table.Column<int>(type: "int", nullable: true),
                    Penny = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exclusivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceExamples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Act = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chapter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    EndYear = table.Column<int>(type: "int", nullable: true),
                    MagicSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MagicStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eras_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Biome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cultures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Politics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPopulation = table.Column<int>(type: "int", nullable: true),
                    DivineMagicians = table.Column<int>(type: "int", nullable: true),
                    WildMagicians = table.Column<int>(type: "int", nullable: true),
                    ParentLocationId = table.Column<int>(type: "int", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotsEras",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    EraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsEras", x => new { x.SnapshotId, x.EraId });
                    table.ForeignKey(
                        name: "FK_SnapshotsEras_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsEras_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LanguagesLocations",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguagesLocations", x => new { x.LanguageId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_LanguagesLocations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LanguagesLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepthMeters = table.Column<int>(type: "int", nullable: true),
                    WidthMeters = table.Column<int>(type: "int", nullable: true),
                    FlowDirection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceLocationId = table.Column<int>(type: "int", nullable: true),
                    DestinationLocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rivers_Locations_DestinationLocationId",
                        column: x => x.DestinationLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rivers_Locations_SourceLocationId",
                        column: x => x.SourceLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TravelTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromId = table.Column<int>(type: "int", nullable: true),
                    ToId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Locations_FromId",
                        column: x => x.FromId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Locations_ToId",
                        column: x => x.ToId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotsLocations",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsLocations", x => new { x.SnapshotId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_SnapshotsLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsLocations_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: true),
                    Weekday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendar_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotsEvents",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsEvents", x => new { x.SnapshotId, x.EventId });
                    table.ForeignKey(
                        name: "FK_SnapshotsEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsEvents_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalendarId = table.Column<int>(type: "int", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true),
                    BookOverride = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterOverride = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlotPoints_Calendar_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPoints_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
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

            migrationBuilder.CreateTable(
                name: "PlotPointsRivers",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    RiverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsRivers", x => new { x.PlotPointId, x.RiverId });
                    table.ForeignKey(
                        name: "FK_PlotPointsRivers_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsRivers_Rivers_RiverId",
                        column: x => x.RiverId,
                        principalTable: "Rivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointsRoutes",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointsRoutes", x => new { x.PlotPointId, x.RouteId });
                    table.ForeignKey(
                        name: "FK_PlotPointsRoutes_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointsRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Effects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
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
                name: "SnapshotsItems",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsItems", x => new { x.SnapshotId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_SnapshotsItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsItems_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Character1Id = table.Column<int>(type: "int", nullable: true),
                    Character2Id = table.Column<int>(type: "int", nullable: true),
                    RelationshipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipDynamic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterRelationships_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "SnapshotsCharacterRelationships",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    CharacterRelationshipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsCharacterRelationships", x => new { x.SnapshotId, x.CharacterRelationshipId });
                    table.ForeignKey(
                        name: "FK_SnapshotsCharacterRelationships_CharacterRelationships_CharacterRelationshipId",
                        column: x => x.CharacterRelationshipId,
                        principalTable: "CharacterRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsCharacterRelationships_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDay = table.Column<int>(type: "int", nullable: true),
                    BirthMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthYear = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeightCm = table.Column<int>(type: "int", nullable: true),
                    Build = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hair = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eyes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefiningFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Magic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Motivation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Misbelief = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FounderId = table.Column<int>(type: "int", nullable: true),
                    LeaderId = table.Column<int>(type: "int", nullable: true),
                    FoundingYear = table.Column<int>(type: "int", nullable: true),
                    HQLocationId = table.Column<int>(type: "int", nullable: true),
                    Magic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factions_Characters_FounderId",
                        column: x => x.FounderId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factions_Characters_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factions_Locations_HQLocationId",
                        column: x => x.HQLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factions_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
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
                name: "SnapshotsCharacters",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsCharacters", x => new { x.SnapshotId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_SnapshotsCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsCharacters_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
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
                name: "SnapshotsFactions",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotsFactions", x => new { x.SnapshotId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_SnapshotsFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotsFactions_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerId",
                table: "Items",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SnapshotId",
                table: "Items",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_EventId",
                table: "Calendar",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_Character1Id",
                table: "CharacterRelationships",
                column: "Character1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_Character2Id",
                table: "CharacterRelationships",
                column: "Character2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_SnapshotId",
                table: "CharacterRelationships",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FactionId",
                table: "Characters",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LanguageId",
                table: "Characters",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LocationId",
                table: "Characters",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SnapshotId",
                table: "Characters",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Eras_SnapshotId",
                table: "Eras",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LocationId",
                table: "Events",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SnapshotId",
                table: "Events",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_FounderId",
                table: "Factions",
                column: "FounderId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_HQLocationId",
                table: "Factions",
                column: "HQLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_LeaderId",
                table: "Factions",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_SnapshotId",
                table: "Factions",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesLocations_LocationId",
                table: "LanguagesLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SnapshotId",
                table: "Locations",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_CalendarId",
                table: "PlotPoints",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_SnapshotId",
                table: "PlotPoints",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsItems_ItemId",
                table: "PlotPointsItems",
                column: "ItemId");

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
                name: "IX_PlotPointsLocations_LocationId",
                table: "PlotPointsLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsRivers_RiverId",
                table: "PlotPointsRivers",
                column: "RiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointsRoutes_RouteId",
                table: "PlotPointsRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Rivers_DestinationLocationId",
                table: "Rivers",
                column: "DestinationLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rivers_SourceLocationId",
                table: "Rivers",
                column: "SourceLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FromId",
                table: "Routes",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ToId",
                table: "Routes",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsItems_ItemId",
                table: "SnapshotsItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsCharacterRelationships_CharacterRelationshipId",
                table: "SnapshotsCharacterRelationships",
                column: "CharacterRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsCharacters_CharacterId",
                table: "SnapshotsCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsEras_EraId",
                table: "SnapshotsEras",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsEvents_EventId",
                table: "SnapshotsEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsFactions_FactionId",
                table: "SnapshotsFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotsLocations_LocationId",
                table: "SnapshotsLocations",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_OwnerId",
                table: "Items",
                column: "OwnerId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterRelationships_Characters_Character1Id",
                table: "CharacterRelationships",
                column: "Character1Id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterRelationships_Characters_Character2Id",
                table: "CharacterRelationships",
                column: "Character2Id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Factions_FactionId",
                table: "Characters",
                column: "FactionId",
                principalTable: "Factions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factions_Characters_FounderId",
                table: "Factions");

            migrationBuilder.DropForeignKey(
                name: "FK_Factions_Characters_LeaderId",
                table: "Factions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "LanguagesLocations");

            migrationBuilder.DropTable(
                name: "PlotPointsItems");

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
                name: "PlotPointsLocations");

            migrationBuilder.DropTable(
                name: "PlotPointsRivers");

            migrationBuilder.DropTable(
                name: "PlotPointsRoutes");

            migrationBuilder.DropTable(
                name: "PriceExamples");

            migrationBuilder.DropTable(
                name: "SnapshotsItems");

            migrationBuilder.DropTable(
                name: "SnapshotsCharacterRelationships");

            migrationBuilder.DropTable(
                name: "SnapshotsCharacters");

            migrationBuilder.DropTable(
                name: "SnapshotsEras");

            migrationBuilder.DropTable(
                name: "SnapshotsEvents");

            migrationBuilder.DropTable(
                name: "SnapshotsFactions");

            migrationBuilder.DropTable(
                name: "SnapshotsLocations");

            migrationBuilder.DropTable(
                name: "Rivers");

            migrationBuilder.DropTable(
                name: "PlotPoints");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "CharacterRelationships");

            migrationBuilder.DropTable(
                name: "Eras");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Snapshots");
        }
    }
}
