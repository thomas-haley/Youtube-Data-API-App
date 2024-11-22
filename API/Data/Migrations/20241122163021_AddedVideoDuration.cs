using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigrations
{
    /// <inheritdoc />
    public partial class AddedVideoDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Video",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Video");
        }
    }
}
