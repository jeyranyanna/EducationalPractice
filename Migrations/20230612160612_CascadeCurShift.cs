using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class CascadeCurShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee",
                table: "current_shift");

            migrationBuilder.DropForeignKey(
                name: "fk_shift",
                table: "current_shift");

            migrationBuilder.AddForeignKey(
                name: "fk_employee",
                table: "current_shift",
                column: "employee",
                principalTable: "staff",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_shift",
                table: "current_shift",
                column: "shift",
                principalTable: "shifts",
                principalColumn: "shift_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee",
                table: "current_shift");

            migrationBuilder.DropForeignKey(
                name: "fk_shift",
                table: "current_shift");


            migrationBuilder.AddForeignKey(
                name: "fk_employee",
                table: "current_shift",
                column: "employee",
                principalTable: "staff",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_shift",
                table: "current_shift",
                column: "shift",
                principalTable: "shifts",
                principalColumn: "shift_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
