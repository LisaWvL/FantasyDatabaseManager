using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyDB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterNavigationFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Event_EventId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ChildLocationsId",
                table: "Location");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Location",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Location_EventId",
                table: "Location",
                newName: "IX_Location_LocationId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId1",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Weekdays",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "MonthsPerYear",
                table: "Calendar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Months",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DaysPerYear",
                table: "Calendar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DaysPerWeek",
                table: "Calendar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LocationId1",
                table: "Event",
                column: "LocationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Location_LocationId1",
                table: "Event",
                column: "LocationId1",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Location_LocationId",
                table: "Location",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Location_LocationId1",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Location_LocationId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Event_LocationId1",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Location",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Location_LocationId",
                table: "Location",
                newName: "IX_Location_EventId");

            migrationBuilder.AddColumn<int>(
                name: "ChildLocationsId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Weekdays",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MonthsPerYear",
                table: "Calendar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Months",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DaysPerYear",
                table: "Calendar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DaysPerWeek",
                table: "Calendar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Event_EventId",
                table: "Location",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
