﻿// <auto-generated />
using System;
using DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(RideManagerContext))]
    partial class RideManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Domain.Entities.ReportDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ReportDates");
                });

            modelBuilder.Entity("Domain.Entities.SystemSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("HoursForPersonalTripStatusChange")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(24);

                    b.HasKey("Id");

                    b.ToTable("SystemSettings");
                });

            modelBuilder.Entity("Domain.Entities.TransportCoordinator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransportCoordinators");
                });

            modelBuilder.Entity("Domain.Entities.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CostWithoutVAT")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<string>("EndAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsPersonal")
                        .HasColumnType("boolean");

                    b.Property<string>("PassengerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PassengerName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("PersonalStatusSetAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReportDateId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<string>("StartAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("VAT")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ReportDateId");

                    b.HasIndex("ReportId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Domain.Entities.Trip", b =>
                {
                    b.HasOne("Domain.Entities.ReportDate", "ReportDate")
                        .WithMany("Trip")
                        .HasForeignKey("ReportDateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Report", "Report")
                        .WithMany("Trip")
                        .HasForeignKey("ReportId");

                    b.Navigation("Report");

                    b.Navigation("ReportDate");
                });

            modelBuilder.Entity("Domain.Entities.Report", b =>
                {
                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.ReportDate", b =>
                {
                    b.Navigation("Trip");
                });
#pragma warning restore 612, 618
        }
    }
}
