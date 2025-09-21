using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Environment_EnvironmentId",
                table: "Deployment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environment",
                table: "Environment");

            migrationBuilder.RenameTable(
                name: "Environment",
                newName: "Environments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environments",
                table: "Environments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Environments_EnvironmentId",
                table: "Deployment",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Environments_EnvironmentId",
                table: "Deployment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environments",
                table: "Environments");

            migrationBuilder.RenameTable(
                name: "Environments",
                newName: "Environment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environment",
                table: "Environment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Environment_EnvironmentId",
                table: "Deployment",
                column: "EnvironmentId",
                principalTable: "Environment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
