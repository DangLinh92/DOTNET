using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateTinhHinhSXoutwf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutPutWafer",
                table: "TINH_HINH_SAN_XUAT_SAMPLE",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutPutWafer",
                table: "TINH_HINH_SAN_XUAT_SAMPLE");
        }
    }
}
