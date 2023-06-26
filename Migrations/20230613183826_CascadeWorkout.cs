using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class CascadeWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trainer",
                table: "current_workout");

            migrationBuilder.DropForeignKey(
                name: "fk_workout_date",
                table: "current_workout");


            migrationBuilder.AddForeignKey(
                name: "fk_trainer",
                table: "current_workout",
                column: "trainer",
                principalTable: "staff",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_workout_date",
                table: "current_workout",
                column: "workout_date",
                principalTable: "workouts",
                principalColumn: "workout_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trainer",
                table: "current_workout");

            migrationBuilder.DropForeignKey(
                name: "fk_workout_date",
                table: "current_workout");

            migrationBuilder.AddForeignKey(
                name: "fk_trainer",
                table: "current_workout",
                column: "trainer",
                principalTable: "staff",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_workout_date",
                table: "current_workout",
                column: "workout_date",
                principalTable: "workouts",
                principalColumn: "workout_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
