using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaEventParent",
                table: "HR_TRAINING",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HR_TRAINING_MaEventParent",
                table: "HR_TRAINING",
                column: "MaEventParent");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_TRAINING_EVENT_SHEDULE_PARENT_MaEventParent",
                table: "HR_TRAINING",
                column: "MaEventParent",
                principalTable: "EVENT_SHEDULE_PARENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_TRAINING_EVENT_SHEDULE_PARENT_MaEventParent",
                table: "HR_TRAINING");

            migrationBuilder.DropIndex(
                name: "IX_HR_TRAINING_MaEventParent",
                table: "HR_TRAINING");

            migrationBuilder.DropColumn(
                name: "MaEventParent",
                table: "HR_TRAINING");
        }
    }
}
