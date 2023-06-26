using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblDelayComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DELAY_COMMENT_SAMPLE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LotNo = table.Column<string>(maxLength: 50, nullable: true),
                    Wall = table.Column<string>(maxLength: 250, nullable: true),
                    Roof = table.Column<string>(maxLength: 250, nullable: true),
                    Seed = table.Column<string>(maxLength: 250, nullable: true),
                    PlatePR = table.Column<string>(maxLength: 250, nullable: true),
                    Plate = table.Column<string>(maxLength: 250, nullable: true),
                    PreProbe = table.Column<string>(maxLength: 250, nullable: true),
                    PreDicing = table.Column<string>(maxLength: 250, nullable: true),
                    AllProbe = table.Column<string>(maxLength: 250, nullable: true),
                    BG = table.Column<string>(maxLength: 250, nullable: true),
                    Dicing = table.Column<string>(maxLength: 250, nullable: true),
                    ChipIns = table.Column<string>(maxLength: 250, nullable: true),
                    Packing = table.Column<string>(maxLength: 250, nullable: true),
                    OQC = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DELAY_COMMENT_SAMPLE", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DELAY_COMMENT_SAMPLE");
        }
    }
}
