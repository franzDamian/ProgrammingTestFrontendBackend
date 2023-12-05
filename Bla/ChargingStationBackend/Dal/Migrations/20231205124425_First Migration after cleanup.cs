using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigrationaftercleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChargingStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChargingPower = table.Column<int>(type: "integer", nullable: false),
                    ChargingValuesForEachDayAndHour = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimulationInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChargingStations = table.Column<string>(type: "text", nullable: false),
                    AverageConsumptionOfCars = table.Column<int>(type: "integer", nullable: false),
                    ArrivalProbabilityMultiplier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationInputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimulationOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChargingStationSimulationResult = table.Column<string>(type: "text", nullable: false),
                    TotalEnergyCharged = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerYear = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerMonth = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerWeek = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerDay = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyFactor = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationOutputs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingStations");

            migrationBuilder.DropTable(
                name: "SimulationInputs");

            migrationBuilder.DropTable(
                name: "SimulationOutputs");
        }
    }
}
