using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addFileMns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FILE_MANAGER",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NCHAR(40)", maxLength: 40, nullable: true),
                    ParentID = table.Column<int>(nullable: false),
                    Size = table.Column<long>(type: "BIGINT", nullable: false),
                    IsFile = table.Column<bool>(type: "BIT", nullable: false),
                    MimeType = table.Column<string>(type: "NCHAR(200)", maxLength: 200, nullable: true),
                    Content = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    HasChild = table.Column<bool>(type: "BIT", nullable: false),
                    IsRoot = table.Column<bool>(type: "BIT", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    FilterPath = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    StorageLocation = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    DateEx = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILE_MANAGER", x => x.ItemID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FILE_MANAGER");
        }
    }
}
