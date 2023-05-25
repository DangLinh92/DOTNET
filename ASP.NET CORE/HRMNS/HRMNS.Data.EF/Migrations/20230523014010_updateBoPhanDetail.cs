using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateBoPhanDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaBoPhan_TOP1",
                table: "HR_BO_PHAN_DETAIL",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaBoPhan_TOP2",
                table: "HR_BO_PHAN_DETAIL",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaBoPhan_TOP1",
                table: "HR_BO_PHAN_DETAIL");

            migrationBuilder.DropColumn(
                name: "MaBoPhan_TOP2",
                table: "HR_BO_PHAN_DETAIL");
        }
    }
}
