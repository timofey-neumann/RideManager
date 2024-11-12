using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HoursForPersonalTripStatusChange = table.Column<int>(type: "integer", nullable: false, defaultValue: 24)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportCoordinators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCoordinators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PassengerEmail = table.Column<string>(type: "text", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CostWithoutVAT = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    VAT = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPersonal = table.Column<bool>(type: "boolean", nullable: false),
                    PersonalStatusSetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReportDateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_ReportDates_ReportDateId",
                        column: x => x.ReportDateId,
                        principalTable: "ReportDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ReportDateId",
                table: "Trips",
                column: "ReportDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ReportId",
                table: "Trips",
                column: "ReportId");

            migrationBuilder.InsertData(
                table: "TransportCoordinators",
                columns: ["Email", "Id"],
                values: ["admin@gmail.com", Guid.NewGuid()]);

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: ["HoursForPersonalTripStatusChange", "Id"],
                values: [24, Guid.NewGuid()]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "TransportCoordinators");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "ReportDates");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
