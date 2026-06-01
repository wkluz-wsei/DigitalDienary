using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddPeselAndGradeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GradeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OldValue = table.Column<int>(type: "INTEGER", nullable: true),
                    NewValue = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedByUserName = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActionType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeHistories_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "Id",
                keyValue: new Guid("05f64714-3ac1-4327-8e02-fd519844126f"),
                column: "NationalId",
                value: "78020254325");

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "Id",
                keyValue: new Guid("0d7c4923-0ed0-4db9-a392-4b3c520ef77e"),
                column: "NationalId",
                value: "75010112346");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                column: "NationalId",
                value: "99010112342");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("7ba19ea5-3967-4b24-bb87-b14c9ee14770"),
                column: "NationalId",
                value: "98020254323");

            migrationBuilder.CreateIndex(
                name: "IX_GradeHistories_GradeId",
                table: "GradeHistories",
                column: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeHistories");

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "Id",
                keyValue: new Guid("05f64714-3ac1-4327-8e02-fd519844126f"),
                column: "NationalId",
                value: "78020254321");

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "Id",
                keyValue: new Guid("0d7c4923-0ed0-4db9-a392-4b3c520ef77e"),
                column: "NationalId",
                value: "75010112345");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                column: "NationalId",
                value: "99010112345");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("7ba19ea5-3967-4b24-bb87-b14c9ee14770"),
                column: "NationalId",
                value: "98020254321");
        }
    }
}
