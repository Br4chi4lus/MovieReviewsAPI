using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewsAPI.Migrations
{
    /// <inheritdoc />
    public partial class reviewdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "content",
                table: "Review",
                newName: "Content");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Review",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfLastModification",
                table: "Review",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "DateOfLastModification",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Review",
                newName: "content");
        }
    }
}
