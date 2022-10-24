using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TimeAlert",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StartEvent",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Repeat",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EndEvent",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BoPhan",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EventDate",
                table: "EVENT_SHEDULE",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TRAINING_TYPE",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainName = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAINING_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_TRAINING",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainnigType = table.Column<Guid>(nullable: false),
                    Trainer = table.Column<string>(maxLength: 250, nullable: true),
                    FromDate = table.Column<string>(maxLength: 50, nullable: true),
                    ToDate = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Cost = table.Column<float>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_TRAINING", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_TRAINING_TRAINING_TYPE_TrainnigType",
                        column: x => x.TrainnigType,
                        principalTable: "TRAINING_TYPE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAINING_NHANVIEN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    TrainnigId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAINING_NHANVIEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRAINING_NHANVIEN_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRAINING_NHANVIEN_HR_TRAINING_TrainnigId",
                        column: x => x.TrainnigId,
                        principalTable: "HR_TRAINING",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_TRAINING_TrainnigType",
                table: "HR_TRAINING",
                column: "TrainnigType");

            migrationBuilder.CreateIndex(
                name: "IX_TRAINING_NHANVIEN_MaNV",
                table: "TRAINING_NHANVIEN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_TRAINING_NHANVIEN_TrainnigId",
                table: "TRAINING_NHANVIEN",
                column: "TrainnigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRAINING_NHANVIEN");

            migrationBuilder.DropTable(
                name: "HR_TRAINING");

            migrationBuilder.DropTable(
                name: "TRAINING_TYPE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TimeAlert",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StartEvent",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Repeat",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EndEvent",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BoPhan",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EventDate",
                table: "EVENT_SHEDULE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
