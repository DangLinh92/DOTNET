using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addSamsungTraining1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAMSUNG_TRAINING",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubNo = table.Column<string>(maxLength: 10, nullable: true),
                    CoursesName = table.Column<string>(maxLength: 150, nullable: true),
                    Level = table.Column<string>(maxLength: 50, nullable: true),
                    Program = table.Column<string>(maxLength: 50, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    Class = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    Week = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    TrainingPlace = table.Column<string>(maxLength: 50, nullable: true),
                    TrainingRoom = table.Column<string>(maxLength: 50, nullable: true),
                    GEN = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Company = table.Column<string>(maxLength: 50, nullable: true),
                    Part = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Score = table.Column<string>(maxLength: 50, nullable: true),
                    Result = table.Column<string>(maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAMSUNG_TRAINING", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAMSUNG_TRAINING");
        }
    }
}
