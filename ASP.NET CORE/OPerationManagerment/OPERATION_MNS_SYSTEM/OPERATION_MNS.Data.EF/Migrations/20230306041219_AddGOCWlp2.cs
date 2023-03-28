﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddGOCWlp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GOC_PLAN_WLP2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    StandardQtyForMonth = table.Column<float>(nullable: false),
                    MonthPlan = table.Column<string>(nullable: true),
                    DatePlan = table.Column<string>(nullable: true),
                    QuantityPlan = table.Column<float>(nullable: false),
                    QuantityActual = table.Column<float>(nullable: false),
                    QuantityGap = table.Column<float>(nullable: false),
                    Department = table.Column<string>(maxLength: 50, nullable: true),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DanhMuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOC_PLAN_WLP2", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GOC_PLAN_WLP2");
        }
    }
}