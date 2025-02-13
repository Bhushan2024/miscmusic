using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class forth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EventLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EventOrganizer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventOrganizerContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBookingAvailable = table.Column<bool>(type: "bit", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    BookedSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
