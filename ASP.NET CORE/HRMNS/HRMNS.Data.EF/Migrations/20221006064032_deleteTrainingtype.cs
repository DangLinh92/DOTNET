using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class deleteTrainingtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_TRAINING_TRAINING_TYPE_TrainnigType",
                table: "HR_TRAINING");

            migrationBuilder.DropTable(
                name: "TRAINING_TYPE");

            migrationBuilder.DropIndex(
                name: "IX_HR_TRAINING_TrainnigType",
                table: "HR_TRAINING");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRAINING_TYPE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrainName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAINING_TYPE", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_TRAINING_TrainnigType",
                table: "HR_TRAINING",
                column: "TrainnigType");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_TRAINING_TRAINING_TYPE_TrainnigType",
                table: "HR_TRAINING",
                column: "TrainnigType",
                principalTable: "TRAINING_TYPE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
