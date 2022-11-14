using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatescheduleEvtParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contents",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndTimezone",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllDay",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurrenceException",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceID",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurrenceRule",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "EVENT_SHEDULE_PARENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTimezone",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "Contents",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "EndTimezone",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "IsAllDay",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "RecurrenceException",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "RecurrenceID",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "RecurrenceRule",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "StartTimezone",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
