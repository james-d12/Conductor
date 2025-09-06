using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ResourceTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ResourceTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
