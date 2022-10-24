using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addEvent1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EVENT_SHEDULE_PARENT",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    StartEvent = table.Column<string>(nullable: true),
                    EndEvent = table.Column<string>(nullable: true),
                    Repeat = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BoPhan = table.Column<string>(nullable: true),
                    TimeAlert = table.Column<string>(nullable: true),
                    MaNoiDungKH = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_SHEDULE_PARENT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EVENT_SHEDULE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDate = table.Column<string>(nullable: true),
                    MaEventParent = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_SHEDULE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EVENT_SHEDULE_EVENT_SHEDULE_PARENT_MaEventParent",
                        column: x => x.MaEventParent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_SHEDULE_MaEventParent",
                table: "EVENT_SHEDULE",
                column: "MaEventParent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EVENT_SHEDULE");

            migrationBuilder.DropTable(
                name: "EVENT_SHEDULE_PARENT");
        }
    }
}
