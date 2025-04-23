using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyDB.Migrations
{
    /// <inheritdoc />
    public partial class NewStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookNumber = table.Column<int>(type: "int", nullable: true),
                    SeriesTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookWordCount = table.Column<int>(type: "int", nullable: true),
                    BookSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookToDo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

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
                name: "Dates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: true),
                    Weekday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => x.Id);
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
                name: "Acts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActNumber = table.Column<int>(type: "int", nullable: true),
                    ActSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActToDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActWordCount = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acts_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
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
                    StartDateId = table.Column<int>(type: "int", nullable: true),
                    EndDateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlotPoints_Dates_EndDateId",
                        column: x => x.EndDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPoints_Dates_StartDateId",
                        column: x => x.StartDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationTurns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DanMode = table.Column<bool>(type: "bit", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSummary = table.Column<bool>(type: "bit", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenCount = table.Column<int>(type: "int", nullable: true),
                    SummaryLevel = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    PlotPointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationTurns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationTurns_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterNumber = table.Column<int>(type: "int", nullable: true),
                    ChapterTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WordCount = table.Column<int>(type: "int", nullable: true),
                    ChapterSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateId = table.Column<int>(type: "int", nullable: true),
                    EndDateId = table.Column<int>(type: "int", nullable: true),
                    ActId = table.Column<int>(type: "int", nullable: true),
                    POVCharacterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Acts_ActId",
                        column: x => x.ActId,
                        principalTable: "Acts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chapters_Dates_EndDateId",
                        column: x => x.EndDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chapters_Dates_StartDateId",
                        column: x => x.StartDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChaptersPlotPoints",
                columns: table => new
                {
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    PlotPointId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChaptersPlotPoints", x => new { x.ChapterId, x.PlotPointId });
                    table.ForeignKey(
                        name: "FK_ChaptersPlotPoints_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChaptersPlotPoints_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Eras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MagicSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MagicStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true),
                    StartDateId = table.Column<int>(type: "int", nullable: true),
                    EndDateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eras_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eras_Dates_EndDateId",
                        column: x => x.EndDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eras_Dates_StartDateId",
                        column: x => x.StartDateId,
                        principalTable: "Dates",
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
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SceneNumber = table.Column<int>(type: "int", nullable: true),
                    SceneTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SceneText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SceneWordCount = table.Column<int>(type: "int", nullable: true),
                    SceneSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SceneToDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenes_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
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
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    StartDateId = table.Column<int>(type: "int", nullable: true),
                    EndDateId = table.Column<int>(type: "int", nullable: true),
                    PlotPointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Dates_EndDateId",
                        column: x => x.EndDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Dates_StartDateId",
                        column: x => x.StartDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
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
                name: "CharacterRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Character1Id = table.Column<int>(type: "int", nullable: true),
                    Character2Id = table.Column<int>(type: "int", nullable: true),
                    RelationshipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipDynamic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterRelationships_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
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
                    ChapterId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    BirthDateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Dates_BirthDateId",
                        column: x => x.BirthDateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FounderId = table.Column<int>(type: "int", nullable: true),
                    LeaderId = table.Column<int>(type: "int", nullable: true),
                    FoundingYear = table.Column<int>(type: "int", nullable: true),
                    HQLocationId = table.Column<int>(type: "int", nullable: true),
                    Magic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factions_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Characters_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acts_BookId",
                table: "Acts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ActId",
                table: "Chapters",
                column: "ActId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_EndDateId",
                table: "Chapters",
                column: "EndDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_POVCharacterId",
                table: "Chapters",
                column: "POVCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_StartDateId",
                table: "Chapters",
                column: "StartDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChaptersPlotPoints_PlotPointId",
                table: "ChaptersPlotPoints",
                column: "PlotPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_ChapterId",
                table: "CharacterRelationships",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_Character1Id",
                table: "CharacterRelationships",
                column: "Character1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_Character2Id",
                table: "CharacterRelationships",
                column: "Character2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BirthDateId",
                table: "Characters",
                column: "BirthDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ChapterId",
                table: "Characters",
                column: "ChapterId");

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
                name: "IX_ConversationTurns_PlotPointId",
                table: "ConversationTurns",
                column: "PlotPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Eras_ChapterId",
                table: "Eras",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Eras_EndDateId",
                table: "Eras",
                column: "EndDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Eras_StartDateId",
                table: "Eras",
                column: "StartDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ChapterId",
                table: "Events",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EndDateId",
                table: "Events",
                column: "EndDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LocationId",
                table: "Events",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PlotPointId",
                table: "Events",
                column: "PlotPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDateId",
                table: "Events",
                column: "StartDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_ChapterId",
                table: "Factions",
                column: "ChapterId");

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
                name: "IX_Items_ChapterId",
                table: "Items",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerId",
                table: "Items",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesLocations_LocationId",
                table: "LanguagesLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ChapterId",
                table: "Locations",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_EndDateId",
                table: "PlotPoints",
                column: "EndDateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_StartDateId",
                table: "PlotPoints",
                column: "StartDateId");

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
                name: "IX_Scenes_ChapterId",
                table: "Scenes",
                column: "ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Characters_POVCharacterId",
                table: "Chapters",
                column: "POVCharacterId",
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
                name: "FK_Acts_Books_BookId",
                table: "Acts");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Acts_ActId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Characters_POVCharacterId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Factions_Characters_FounderId",
                table: "Factions");

            migrationBuilder.DropForeignKey(
                name: "FK_Factions_Characters_LeaderId",
                table: "Factions");

            migrationBuilder.DropTable(
                name: "ChaptersPlotPoints");

            migrationBuilder.DropTable(
                name: "CharacterRelationships");

            migrationBuilder.DropTable(
                name: "ConversationTurns");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Eras");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "LanguagesLocations");

            migrationBuilder.DropTable(
                name: "PlotPointsRivers");

            migrationBuilder.DropTable(
                name: "PlotPointsRoutes");

            migrationBuilder.DropTable(
                name: "PriceExamples");

            migrationBuilder.DropTable(
                name: "Scenes");

            migrationBuilder.DropTable(
                name: "Rivers");

            migrationBuilder.DropTable(
                name: "PlotPoints");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Acts");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Dates");
        }
    }
}
