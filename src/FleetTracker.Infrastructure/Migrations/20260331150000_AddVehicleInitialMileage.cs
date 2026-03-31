using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleInitialMileage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InitialMileageKm",
                table: "Vehicles",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "InitialMileageRecordedAtUtc",
                table: "Vehicles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialMileageKm",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InitialMileageRecordedAtUtc",
                table: "Vehicles");
        }
    }
}
