using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updatePostOperationShiping3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KetQuaFAKiemTra",
                table: "POST_OPERATION_SHIPPING",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KetQuaFAKiemTra",
                table: "POST_OPERATION_SHIPPING");
        }
    }
}
