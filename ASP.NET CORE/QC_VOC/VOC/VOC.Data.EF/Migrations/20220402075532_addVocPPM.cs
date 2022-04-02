using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class addVocPPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VOC_PPM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOC_PPM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VOC_PPM");
        }
    }
}
