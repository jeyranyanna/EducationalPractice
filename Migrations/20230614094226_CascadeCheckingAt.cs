using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class CascadeCheckingAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_attraction_id",
                table: "checking_attractions");

            migrationBuilder.AddForeignKey(
                name: "fk_attraction_id",
                table: "checking_attractions",
                column: "attraction_id",
                principalTable: "attractions",
                principalColumn: "attraction_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_attraction_id",
                table: "checking_attractions");

            migrationBuilder.AddForeignKey(
                name: "fk_attraction_id",
                table: "checking_attractions",
                column: "attraction_id",
                principalTable: "attractions",
                principalColumn: "attraction_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
