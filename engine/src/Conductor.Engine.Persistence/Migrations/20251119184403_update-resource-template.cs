using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Engine.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateresourcetemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTemplateVersion",
                table: "ResourceTemplateVersion");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ResourceTemplateVersion",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTemplateVersion",
                table: "ResourceTemplateVersion",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTemplateVersion",
                table: "ResourceTemplateVersion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ResourceTemplateVersion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTemplateVersion",
                table: "ResourceTemplateVersion",
                columns: new[] { "TemplateId", "Version" });
        }
    }
}
