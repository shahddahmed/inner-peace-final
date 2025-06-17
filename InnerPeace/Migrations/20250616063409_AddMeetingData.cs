using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnerPeace.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostingUrl",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ViewerUrl",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostingUrl",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ViewerUrl",
                table: "Sessions");
        }
    }
}
