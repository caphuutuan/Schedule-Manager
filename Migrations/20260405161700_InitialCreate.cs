using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScheduleManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Trường THPT Nguyễn Du" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Grade", "Name", "SchoolId" },
                values: new object[,]
                {
                    { 1, 10, "10A1", 1 },
                    { 2, 11, "11B2", 1 },
                    { 3, 12, "12C3", 1 }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name", "SchoolId" },
                values: new object[,]
                {
                    { 1, "Tổ Toán - Lý - Tin", 1 },
                    { 2, "Tổ Văn - Sử - Địa", 1 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "DepartmentId", "Name", "SchoolId" },
                values: new object[,]
                {
                    { 1, 1, "Nguyễn Văn An", 1 },
                    { 2, 1, "Trần Thị Bình", 1 },
                    { 3, 1, "Lê Văn Cường", 1 },
                    { 4, 2, "Phạm Thị Dung", 1 },
                    { 5, 2, "Hoàng Văn Ê", 1 }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ClassId", "DayOfWeek", "EndTime", "SchoolId", "StartTime", "Subject", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1, 1, new TimeSpan(0, 8, 30, 0, 0), 1, new TimeSpan(0, 7, 0, 0, 0), "Toán", 1 },
                    { 2, 1, 1, new TimeSpan(0, 10, 30, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), "Vật Lý", 2 },
                    { 3, 2, 2, new TimeSpan(0, 8, 30, 0, 0), 1, new TimeSpan(0, 7, 0, 0, 0), "Tin Học", 3 },
                    { 4, 2, 2, new TimeSpan(0, 10, 30, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), "Ngữ Văn", 4 },
                    { 5, 3, 3, new TimeSpan(0, 8, 30, 0, 0), 1, new TimeSpan(0, 7, 0, 0, 0), "Lịch Sử", 5 },
                    { 6, 1, 3, new TimeSpan(0, 10, 30, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), "Toán", 1 },
                    { 7, 2, 4, new TimeSpan(0, 8, 30, 0, 0), 1, new TimeSpan(0, 7, 0, 0, 0), "Vật Lý", 2 },
                    { 8, 3, 4, new TimeSpan(0, 10, 30, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), "Tin Học", 3 },
                    { 9, 1, 5, new TimeSpan(0, 8, 30, 0, 0), 1, new TimeSpan(0, 7, 0, 0, 0), "Ngữ Văn", 4 },
                    { 10, 2, 5, new TimeSpan(0, 10, 30, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), "Địa Lý", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolId",
                table: "Departments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ClassId",
                table: "Schedules",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SchoolId",
                table: "Schedules",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TeacherId",
                table: "Schedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
