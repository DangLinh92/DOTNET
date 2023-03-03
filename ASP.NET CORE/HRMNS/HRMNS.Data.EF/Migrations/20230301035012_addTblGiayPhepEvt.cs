using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblGiayPhepEvt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EHS_QUANLY_GIAY_PHEP_MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP",
                column: "MaEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_EHS_QUANLY_GIAY_PHEP_EVENT_SHEDULE_PARENT_MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP",
                column: "MaEvent",
                principalTable: "EVENT_SHEDULE_PARENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EHS_QUANLY_GIAY_PHEP_EVENT_SHEDULE_PARENT_MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP");

            migrationBuilder.DropIndex(
                name: "IX_EHS_QUANLY_GIAY_PHEP_MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP");

            migrationBuilder.DropColumn(
                name: "MaEvent",
                table: "EHS_QUANLY_GIAY_PHEP");
        }
    }
}
