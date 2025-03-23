using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyDB.Migrations
{
    /// <inheritdoc />
    public partial class NewLayoutWithFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weekdays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Months = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaysPerWeek = table.Column<int>(type: "int", nullable: false),
                    MonthsPerYear = table.Column<int>(type: "int", nullable: false),
                    DaysPerYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crown = table.Column<int>(type: "int", nullable: true),
                    Shilling = table.Column<int>(type: "int", nullable: true),
                    Penny = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceExample",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exclusivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceExample", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Snapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Act = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chapter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Era",
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
                    table.PrimaryKey("PK_Era", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Era_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotEra",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    EraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotEra", x => new { x.SnapshotId, x.EraId });
                    table.ForeignKey(
                        name: "FK_SnapshotEra_Era_EraId",
                        column: x => x.EraId,
                        principalTable: "Era",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotEra_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artifact",
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
                    table.PrimaryKey("PK_Artifact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artifact_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotArtifact",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    ArtifactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotArtifact", x => new { x.SnapshotId, x.ArtifactId });
                    table.ForeignKey(
                        name: "FK_SnapshotArtifact_Artifact_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotArtifact_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Character",
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
                    table.PrimaryKey("PK_Character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Character_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterRelationship",
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
                    table.PrimaryKey("PK_CharacterRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterRelationship_Character_Character1Id",
                        column: x => x.Character1Id,
                        principalTable: "Character",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CharacterRelationship_Character_Character2Id",
                        column: x => x.Character2Id,
                        principalTable: "Character",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CharacterRelationship_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotCharacter",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotCharacter", x => new { x.SnapshotId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_SnapshotCharacter_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotCharacter_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotCharacterRelationship",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    CharacterRelationshipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotCharacterRelationship", x => new { x.SnapshotId, x.CharacterRelationshipId });
                    table.ForeignKey(
                        name: "FK_SnapshotCharacterRelationship_CharacterRelationship_CharacterRelationshipId",
                        column: x => x.CharacterRelationshipId,
                        principalTable: "CharacterRelationship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotCharacterRelationship_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
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
                    LocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotEvent",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotEvent", x => new { x.SnapshotId, x.EventId });
                    table.ForeignKey(
                        name: "FK_SnapshotEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotEvent_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Faction",
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
                    table.PrimaryKey("PK_Faction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faction_Character_FounderId",
                        column: x => x.FounderId,
                        principalTable: "Character",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Faction_Character_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Character",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Faction_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotFaction",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotFaction", x => new { x.SnapshotId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_SnapshotFaction_Faction_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Faction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotFaction_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
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
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    SnapshotId = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Location_Location_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Location_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LocationEvent",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationEvent", x => new { x.LocationId, x.EventId });
                    table.ForeignKey(
                        name: "FK_LocationEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationEvent_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageLocation",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLocation", x => new { x.LocationId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_LanguageLocation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationLocation",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ChildLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationLocation", x => new { x.LocationId, x.ChildLocationId });
                    table.ForeignKey(
                        name: "FK_LocationLocation_Location_ChildLocationId",
                        column: x => x.ChildLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LocationLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "River",
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
                    table.PrimaryKey("PK_River", x => x.Id);
                    table.ForeignKey(
                        name: "FK_River_Location_DestinationLocationId",
                        column: x => x.DestinationLocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_River_Location_SourceLocationId",
                        column: x => x.SourceLocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Route",
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
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_Location_FromId",
                        column: x => x.FromId,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Route_Location_ToId",
                        column: x => x.ToId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SnapshotLocation",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotLocation", x => new { x.SnapshotId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_SnapshotLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotLocation_Snapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artifact_OwnerId",
                table: "Artifact",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifact_SnapshotId",
                table: "Artifact",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_FactionId",
                table: "Character",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_LanguageId",
                table: "Character",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_LocationId",
                table: "Character",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_SnapshotId",
                table: "Character",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationship_Character1Id",
                table: "CharacterRelationship",
                column: "Character1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationship_Character2Id",
                table: "CharacterRelationship",
                column: "Character2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationship_SnapshotId",
                table: "CharacterRelationship",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Era_SnapshotId",
                table: "Era",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LocationId",
                table: "Event",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_SnapshotId",
                table: "Event",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Faction_FounderId",
                table: "Faction",
                column: "FounderId");

            migrationBuilder.CreateIndex(
                name: "IX_Faction_HQLocationId",
                table: "Faction",
                column: "HQLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Faction_LeaderId",
                table: "Faction",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Faction_SnapshotId",
                table: "Faction",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_LocationId",
                table: "Language",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_EventId",
                table: "Location",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_LanguageId",
                table: "Location",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ParentLocationId",
                table: "Location",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_SnapshotId",
                table: "Location",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationEvent_EventId",
                table: "LocationEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLocation_LanguageId",
                table: "LanguageLocation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationLocation_ChildLocationId",
                table: "LocationLocation",
                column: "ChildLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_River_DestinationLocationId",
                table: "River",
                column: "DestinationLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_River_SourceLocationId",
                table: "River",
                column: "SourceLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_FromId",
                table: "Route",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_ToId",
                table: "Route",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotArtifact_ArtifactId",
                table: "SnapshotArtifact",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotCharacter_CharacterId",
                table: "SnapshotCharacter",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotCharacterRelationship_CharacterRelationshipId",
                table: "SnapshotCharacterRelationship",
                column: "CharacterRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotEra_EraId",
                table: "SnapshotEra",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotEvent_EventId",
                table: "SnapshotEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotFaction_FactionId",
                table: "SnapshotFaction",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotLocation_LocationId",
                table: "SnapshotLocation",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artifact_Character_OwnerId",
                table: "Artifact",
                column: "OwnerId",
                principalTable: "Character",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Faction_FactionId",
                table: "Character",
                column: "FactionId",
                principalTable: "Faction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Language_LanguageId",
                table: "Character",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Location_LocationId",
                table: "Character",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Location_LocationId",
                table: "Event",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faction_Location_HQLocationId",
                table: "Faction",
                column: "HQLocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_Location_LocationId",
                table: "Language",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faction_Character_FounderId",
                table: "Faction");

            migrationBuilder.DropForeignKey(
                name: "FK_Faction_Character_LeaderId",
                table: "Faction");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Snapshot_SnapshotId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Snapshot_SnapshotId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Language_LanguageId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Location_LocationId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "LocationEvent");

            migrationBuilder.DropTable(
                name: "LanguageLocation");

            migrationBuilder.DropTable(
                name: "LocationLocation");

            migrationBuilder.DropTable(
                name: "PriceExample");

            migrationBuilder.DropTable(
                name: "River");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "SnapshotArtifact");

            migrationBuilder.DropTable(
                name: "SnapshotCharacter");

            migrationBuilder.DropTable(
                name: "SnapshotCharacterRelationship");

            migrationBuilder.DropTable(
                name: "SnapshotEra");

            migrationBuilder.DropTable(
                name: "SnapshotEvent");

            migrationBuilder.DropTable(
                name: "SnapshotFaction");

            migrationBuilder.DropTable(
                name: "SnapshotLocation");

            migrationBuilder.DropTable(
                name: "Artifact");

            migrationBuilder.DropTable(
                name: "CharacterRelationship");

            migrationBuilder.DropTable(
                name: "Era");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "Faction");

            migrationBuilder.DropTable(
                name: "Snapshot");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
