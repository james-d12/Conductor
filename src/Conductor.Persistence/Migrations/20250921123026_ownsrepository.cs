using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ownsrepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Repository_RepositoryId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "Repository");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RepositoryId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "RepositoryId",
                table: "Applications",
                newName: "Repository_Url");

            migrationBuilder.AddColumn<Guid>(
                name: "Repository_Id",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Repository_Name",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Repository_Provider",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repository_Id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Repository_Name",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Repository_Provider",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "Repository_Url",
                table: "Applications",
                newName: "RepositoryId");

            migrationBuilder.CreateTable(
                name: "Repository",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Provider = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repository", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RepositoryId",
                table: "Applications",
                column: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Repository_RepositoryId",
                table: "Applications",
                column: "RepositoryId",
                principalTable: "Repository",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
