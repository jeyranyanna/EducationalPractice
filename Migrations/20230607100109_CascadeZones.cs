using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class CascadeZones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_zone_location",
                table: "attractions");

            migrationBuilder.AddForeignKey(
                name: "fk_zone_location",
                table: "attractions",
                column: "zone_location",
                principalTable: "zones",
                principalColumn: "zone_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_zone_location",
                table: "attractions");

            migrationBuilder.AddForeignKey(
                name: "fk_zone_location",
                table: "attractions",
                column: "zone_location",
                principalTable: "zones",
                principalColumn: "zone_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
