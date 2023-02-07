using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateKhamSK1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NguoiPhuTrach",
                table: "EHS_KE_HOACH_KHAM_SK",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhaThau",
                table: "EHS_KE_HOACH_KHAM_SK",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiPhuTrach",
                table: "EHS_KE_HOACH_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "NhaThau",
                table: "EHS_KE_HOACH_KHAM_SK");
        }
    }
}
