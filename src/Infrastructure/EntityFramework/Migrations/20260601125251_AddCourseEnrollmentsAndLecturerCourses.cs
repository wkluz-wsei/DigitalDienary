using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseEnrollmentsAndLecturerCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => new { x.CourseId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LecturerCourses",
                columns: table => new
                {
                    LecturerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerCourses", x => new { x.LecturerId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_LecturerCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LecturerCourses_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CourseEnrollments",
                columns: new[] { "CourseId", "StudentId" },
                values: new object[,]
                {
                    { new Guid("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2"), new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f") },
                    { new Guid("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2"), new Guid("7ba19ea5-3967-4b24-bb87-b14c9ee14770") },
                    { new Guid("8e8f8f66-bb84-4f77-8a42-6052b8c64410"), new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f") }
                });

            migrationBuilder.InsertData(
                table: "LecturerCourses",
                columns: new[] { "CourseId", "LecturerId" },
                values: new object[,]
                {
                    { new Guid("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2"), new Guid("05f64714-3ac1-4327-8e02-fd519844126f") },
                    { new Guid("8e8f8f66-bb84-4f77-8a42-6052b8c64410"), new Guid("0d7c4923-0ed0-4db9-a392-4b3c520ef77e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StudentId",
                table: "CourseEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerCourses_CourseId",
                table: "LecturerCourses",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEnrollments");

            migrationBuilder.DropTable(
                name: "LecturerCourses");
        }
    }
}
