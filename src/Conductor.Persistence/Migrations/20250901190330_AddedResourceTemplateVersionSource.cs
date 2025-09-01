using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedResourceTemplateVersionSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Source",
                table: "ResourceTemplateVersion",
                newName: "Source_FolderPath");

            migrationBuilder.AddColumn<string>(
                name: "Source_BaseUrl",
                table: "ResourceTemplateVersion",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source_BaseUrl",
                table: "ResourceTemplateVersion");

            migrationBuilder.RenameColumn(
                name: "Source_FolderPath",
                table: "ResourceTemplateVersion",
                newName: "Source");
        }
    }
}
