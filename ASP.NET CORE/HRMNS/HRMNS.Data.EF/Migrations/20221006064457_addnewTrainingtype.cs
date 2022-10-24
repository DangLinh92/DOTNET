using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addnewTrainingtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainnigType",
                table: "HR_TRAINING",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TRAINING_TYPE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainName = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_TRAINING_TRAINING_TYPE_TrainnigType",
                table: "HR_TRAINING");

            migrationBuilder.DropTable(
                name: "TRAINING_TYPE");

            migrationBuilder.DropIndex(
                name: "IX_HR_TRAINING_TrainnigType",
                table: "HR_TRAINING");

            migrationBuilder.DropColumn(
                name: "TrainnigType",
                table: "HR_TRAINING");
        }
    }
}
