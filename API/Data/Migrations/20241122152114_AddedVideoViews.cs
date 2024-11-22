using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigrations
{
    /// <inheritdoc />
    public partial class AddedVideoViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFetched",
                table: "Video",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Views",
                table: "Video",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFetched",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Video");
        }
    }
}
