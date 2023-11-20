﻿// <auto-generated />
using System;
using Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.Migrations
{
    [DbContext(typeof(ChargingStationContext))]
    [Migration("20231117123543_RemoveValueGenerationInModelBuilder")]
    partial class RemoveValueGenerationInModelBuilder
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dal.Model.ChargingStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChargingPower")
                        .HasColumnType("integer");

                    b.Property<int?>("SimulationInputid")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SimulationInputid");

                    b.ToTable("ChargingStations");
                });

            modelBuilder.Entity("Dal.Model.SimulationInput", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("ArrivalProbabilityMultiplier")
                        .HasColumnType("integer");

                    b.Property<int>("AverageConsumptionOfCars")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("SimulationInputs");
                });

            modelBuilder.Entity("Dal.Model.SimulationOutput", b =>
                {
                    b.Property<string>("ChargingValuesPerChargingStationPerDay")
                        .HasColumnType("text");

                    b.Property<double>("DeviationOfConcurrencyFactor")
                        .HasColumnType("double precision");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfChargingEventsPerDay")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfChargingEventsPerMonth")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfChargingEventsPerWeek")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfChargingEventsPerYear")
                        .HasColumnType("integer");

                    b.Property<int>("TotalEnergyCharged")
                        .HasColumnType("integer");

                    b.HasKey("ChargingValuesPerChargingStationPerDay");

                    b.ToTable("SimulationOutputs");
                });

            modelBuilder.Entity("Dal.Model.ChargingStation", b =>
                {
                    b.HasOne("Dal.Model.SimulationInput", null)
                        .WithMany("ChargingStations")
                        .HasForeignKey("SimulationInputid");
                });

            modelBuilder.Entity("Dal.Model.SimulationInput", b =>
                {
                    b.Navigation("ChargingStations");
                });
#pragma warning restore 612, 618
        }
    }
}
