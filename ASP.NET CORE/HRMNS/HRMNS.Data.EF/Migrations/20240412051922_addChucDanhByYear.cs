using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addChucDanhByYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaChucDanhEN",
                table: "HR_CHUCDANH");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "HR_CHUCDANH");

            migrationBuilder.CreateTable(
                name: "HR_CHUCDANH_BY_YEAR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChucDanh = table.Column<string>(maxLength: 250, nullable: true),
                    TenChucDanh = table.Column<string>(maxLength: 50, nullable: true),
                    PhuCap = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CHUCDANH_BY_YEAR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_CHUCDANH_BY_YEAR");

            migrationBuilder.AddColumn<string>(
                name: "MaChucDanhEN",
                table: "HR_CHUCDANH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "HR_CHUCDANH",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
