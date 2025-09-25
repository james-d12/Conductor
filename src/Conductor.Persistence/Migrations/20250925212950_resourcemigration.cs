using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class resourcemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deployments_ApplicationId",
                table: "Deployments");

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceTemplateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EnvironmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTemplateVersion_TemplateId_Version",
                table: "ResourceTemplateVersion",
                columns: new[] { "TemplateId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Environments_Name",
                table: "Environments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_ApplicationId_EnvironmentId_CommitId_Status",
                table: "Deployments",
                columns: new[] { "ApplicationId", "EnvironmentId", "CommitId", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ApplicationId_EnvironmentId_ResourceTemplateId",
                table: "Resource",
                columns: new[] { "ApplicationId", "EnvironmentId", "ResourceTemplateId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_ResourceTemplateVersion_TemplateId_Version",
                table: "ResourceTemplateVersion");

            migrationBuilder.DropIndex(
                name: "IX_Environments_Name",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Deployments_ApplicationId_EnvironmentId_CommitId_Status",
                table: "Deployments");

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_ApplicationId",
                table: "Deployments",
                column: "ApplicationId");
        }
    }
}
