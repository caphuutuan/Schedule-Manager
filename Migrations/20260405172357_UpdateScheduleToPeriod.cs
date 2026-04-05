using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleToPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "Period",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                column: "Period",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                column: "Period",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 4,
                column: "Period",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 5,
                column: "Period",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 6,
                column: "Period",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 7,
                column: "Period",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 8,
                column: "Period",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 9,
                column: "Period",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 10,
                column: "Period",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "Schedules");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 8, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 8, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 8, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 8, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 8, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });
        }
    }
}
