using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblSetingTimeClviec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SETTING_TIME_CA_LVIEC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartWorking = table.Column<string>(maxLength: 50, nullable: true),
                    EndWorking = table.Column<string>(maxLength: 50, nullable: true),
                    BeginTimeOT = table.Column<string>(maxLength: 50, nullable: true),
                    CaLamViec = table.Column<string>(maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SETTING_TIME_CA_LVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SETTING_TIME_CA_LVIEC_DM_CA_LVIEC_CaLamViec",
                        column: x => x.CaLamViec,
                        principalTable: "DM_CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SETTING_TIME_CA_LVIEC_CaLamViec",
                table: "SETTING_TIME_CA_LVIEC",
                column: "CaLamViec");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SETTING_TIME_CA_LVIEC");
        }
    }
}
