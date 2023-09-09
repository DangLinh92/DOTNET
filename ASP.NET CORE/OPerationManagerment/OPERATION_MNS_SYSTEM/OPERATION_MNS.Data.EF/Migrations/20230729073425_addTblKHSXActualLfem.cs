using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblKHSXActualLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KHSX_ACTUAL_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    MesItemId = table.Column<string>(maxLength: 50, nullable: true),
                    OperationId = table.Column<string>(maxLength: 50, nullable: true),
                    DateActual = table.Column<string>(maxLength: 50, nullable: true),
                    WeekActual = table.Column<string>(maxLength: 50, nullable: true),
                    QuantityActual = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHSX_ACTUAL_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KHSX_ACTUAL_LFEM");
        }
    }
}
