using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conductor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deployments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Applications_ApplicationId",
                table: "Deployment");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Environments_EnvironmentId",
                table: "Deployment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deployment",
                table: "Deployment");

            migrationBuilder.RenameTable(
                name: "Deployment",
                newName: "Deployments");

            migrationBuilder.RenameIndex(
                name: "IX_Deployment_EnvironmentId",
                table: "Deployments",
                newName: "IX_Deployments_EnvironmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Deployment_ApplicationId",
                table: "Deployments",
                newName: "IX_Deployments_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deployments",
                table: "Deployments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Applications_ApplicationId",
                table: "Deployments",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Environments_EnvironmentId",
                table: "Deployments",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Applications_ApplicationId",
                table: "Deployments");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Environments_EnvironmentId",
                table: "Deployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deployments",
                table: "Deployments");

            migrationBuilder.RenameTable(
                name: "Deployments",
                newName: "Deployment");

            migrationBuilder.RenameIndex(
                name: "IX_Deployments_EnvironmentId",
                table: "Deployment",
                newName: "IX_Deployment_EnvironmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Deployments_ApplicationId",
                table: "Deployment",
                newName: "IX_Deployment_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deployment",
                table: "Deployment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Applications_ApplicationId",
                table: "Deployment",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Environments_EnvironmentId",
                table: "Deployment",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
