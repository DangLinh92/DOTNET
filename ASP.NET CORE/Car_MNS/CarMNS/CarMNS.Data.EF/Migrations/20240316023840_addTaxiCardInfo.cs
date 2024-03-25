using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class addTaxiCardInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxiCardNo",
                table: "DANG_KY_XE_TAXI",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MUCDICHSD_XE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MucDich = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUCDICHSD_XE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TAXI_CARD_INFO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNo = table.Column<string>(maxLength: 50, nullable: true),
                    CardName = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAXI_CARD_INFO", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MUCDICHSD_XE");

            migrationBuilder.DropTable(
                name: "TAXI_CARD_INFO");

            migrationBuilder.DropColumn(
                name: "TaxiCardNo",
                table: "DANG_KY_XE_TAXI");
        }
    }
}
