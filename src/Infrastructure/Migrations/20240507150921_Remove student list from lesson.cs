using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removestudentlistfromlesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonUsersPending_Lesson_WaitingListId",
                table: "LessonUsersPending");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonUsersPending_User_WaitingListId1",
                table: "LessonUsersPending");

            migrationBuilder.DropTable(
                name: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "MaxStudent",
                table: "Lesson");

            migrationBuilder.AlterColumn<int>(
                name: "WaitingListId1",
                table: "LessonUsersPending",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "WaitingListId",
                table: "LessonUsersPending",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Lesson",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_StudentId",
                table: "Lesson",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_User_StudentId",
                table: "Lesson",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonUsersPending_Lesson_WaitingListId1",
                table: "LessonUsersPending",
                column: "WaitingListId1",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonUsersPending_User_WaitingListId",
                table: "LessonUsersPending",
                column: "WaitingListId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_User_StudentId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonUsersPending_Lesson_WaitingListId1",
                table: "LessonUsersPending");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonUsersPending_User_WaitingListId",
                table: "LessonUsersPending");

            migrationBuilder.DropIndex(
                name: "IX_Lesson_StudentId",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Lesson");

            migrationBuilder.AlterColumn<Guid>(
                name: "WaitingListId1",
                table: "LessonUsersPending",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "WaitingListId",
                table: "LessonUsersPending",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "MaxStudent",
                table: "Lesson",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LessonStudents",
                columns: table => new
                {
                    LessonsAsStudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonStudents", x => new { x.LessonsAsStudentId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_LessonStudents_Lesson_LessonsAsStudentId",
                        column: x => x.LessonsAsStudentId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonStudents_User_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_StudentsId",
                table: "LessonStudents",
                column: "StudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonUsersPending_Lesson_WaitingListId",
                table: "LessonUsersPending",
                column: "WaitingListId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonUsersPending_User_WaitingListId1",
                table: "LessonUsersPending",
                column: "WaitingListId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
