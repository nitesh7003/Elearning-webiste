using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapplication.Migrations
{
    /// <inheritdoc />
    public partial class Assignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentFilePath",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Assignments",
                newName: "QuizId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Assignments",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Assignments",
                newName: "ContentType");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "Assignments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "Assignments",
                newName: "TopicId");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Assignments",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Assignments",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentFilePath",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
