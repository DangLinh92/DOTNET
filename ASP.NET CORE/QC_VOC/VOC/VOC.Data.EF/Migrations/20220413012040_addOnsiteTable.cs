using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class addOnsiteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VOC_ONSITE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(nullable: false),
                    Week = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    Part = table.Column<string>(maxLength: 50, nullable: true),
                    Customer_Code = table.Column<string>(maxLength: 50, nullable: true),
                    Wisol_Model = table.Column<string>(maxLength: 50, nullable: true),
                    Customer = table.Column<string>(maxLength: 50, nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    Marking = table.Column<string>(maxLength: 250, nullable: true),
                    SetModel = table.Column<string>(maxLength: 50, nullable: true),
                    OK = table.Column<string>(maxLength: 10, nullable: true),
                    NG = table.Column<string>(maxLength: 10, nullable: true),
                    Not_Measure = table.Column<string>(maxLength: 10, nullable: true),
                    ProductionDate = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOC_ONSITE", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VOC_ONSITE");
        }
    }
}
