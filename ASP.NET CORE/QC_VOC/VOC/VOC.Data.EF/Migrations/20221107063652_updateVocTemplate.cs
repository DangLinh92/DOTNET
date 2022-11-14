using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class updateVocTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerGroup",
                table: "VOC_MST_BACKUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdutionDate",
                table: "VOC_MST_BACKUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdutionDate_2",
                table: "VOC_MST_BACKUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceivedDate_2",
                table: "VOC_MST_BACKUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SPLReceivedDate_2",
                table: "VOC_MST_BACKUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerGroup",
                table: "VOC_MST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdutionDate",
                table: "VOC_MST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdutionDate_2",
                table: "VOC_MST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceivedDate_2",
                table: "VOC_MST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SPLReceivedDate_2",
                table: "VOC_MST",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerGroup",
                table: "VOC_MST_BACKUP");

            migrationBuilder.DropColumn(
                name: "ProdutionDate",
                table: "VOC_MST_BACKUP");

            migrationBuilder.DropColumn(
                name: "ProdutionDate_2",
                table: "VOC_MST_BACKUP");

            migrationBuilder.DropColumn(
                name: "ReceivedDate_2",
                table: "VOC_MST_BACKUP");

            migrationBuilder.DropColumn(
                name: "SPLReceivedDate_2",
                table: "VOC_MST_BACKUP");

            migrationBuilder.DropColumn(
                name: "CustomerGroup",
                table: "VOC_MST");

            migrationBuilder.DropColumn(
                name: "ProdutionDate",
                table: "VOC_MST");

            migrationBuilder.DropColumn(
                name: "ProdutionDate_2",
                table: "VOC_MST");

            migrationBuilder.DropColumn(
                name: "ReceivedDate_2",
                table: "VOC_MST");

            migrationBuilder.DropColumn(
                name: "SPLReceivedDate_2",
                table: "VOC_MST");
        }
    }
}
