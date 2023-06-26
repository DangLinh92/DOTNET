using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateTinhhinhsxTcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanInputDateTcard",
                table: "TINH_HINH_SAN_XUAT_SAMPLE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanInputDateTcard",
                table: "TINH_HINH_SAN_XUAT_SAMPLE");
        }
    }
}
