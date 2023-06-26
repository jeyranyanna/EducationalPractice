using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_post_id",
                table: "staff");

            migrationBuilder.AddForeignKey(
                name: "fk_post_id",
                table: "staff",
                column: "post",
                principalTable: "posts",
                principalColumn: "post_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_post_id",
                table: "staff");

            migrationBuilder.AddForeignKey(
                name: "fk_post_id",
                table: "staff",
                column: "post",
                principalTable: "posts",
                principalColumn: "post_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
