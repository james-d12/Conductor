using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Engine.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrganisationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Environments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Environments_OrganisationId",
                table: "Environments",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OrganisationId",
                table: "Applications",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Organisation_OrganisationId",
                table: "Applications",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Organisation_OrganisationId",
                table: "Environments",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Organisation_OrganisationId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Organisation_OrganisationId",
                table: "Environments");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropIndex(
                name: "IX_Environments_OrganisationId",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Applications_OrganisationId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Applications");
        }
    }
}
