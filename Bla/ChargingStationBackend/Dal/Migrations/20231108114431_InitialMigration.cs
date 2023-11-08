using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimulationInputs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AverageConsumptionOfCars = table.Column<int>(type: "integer", nullable: false),
                    ArrivalProbabilityMultiplier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationInputs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SimulationOutputs",
                columns: table => new
                {
                    ChargingValuesPerChargingStationPerDay = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalEnergyCharged = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerYear = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerMonth = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerWeek = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChargingEventsPerDay = table.Column<int>(type: "integer", nullable: false),
                    DeviationOfConcurrencyFactor = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationOutputs", x => x.ChargingValuesPerChargingStationPerDay);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChargingPower = table.Column<int>(type: "integer", nullable: false),
                    SimulationInputid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStations_SimulationInputs_SimulationInputid",
                        column: x => x.SimulationInputid,
                        principalTable: "SimulationInputs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_SimulationInputid",
                table: "ChargingStations",
                column: "SimulationInputid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingStations");

            migrationBuilder.DropTable(
                name: "SimulationOutputs");

            migrationBuilder.DropTable(
                name: "SimulationInputs");
        }
    }
}
