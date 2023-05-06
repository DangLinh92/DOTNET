using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class UpdateTblCTQWlp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LWL",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.DropColumn(
                name: "UWL",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.AddColumn<double>(
                name: "MaxV",
                table: "CTQ_SETTING_WLP2",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinV",
                table: "CTQ_SETTING_WLP2",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SpeacialModel",
                table: "CTQ_SETTING_WLP2",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ThickNet",
                table: "CTQ_SETTING_WLP2",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxV",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.DropColumn(
                name: "MinV",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.DropColumn(
                name: "SpeacialModel",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.DropColumn(
                name: "ThickNet",
                table: "CTQ_SETTING_WLP2");

            migrationBuilder.AddColumn<double>(
                name: "LWL",
                table: "CTQ_SETTING_WLP2",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UWL",
                table: "CTQ_SETTING_WLP2",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
