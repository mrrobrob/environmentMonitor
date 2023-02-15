﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using environmentMonitor.Data;

#nullable disable

namespace environmentMonitor.Migrations
{
    [DbContext(typeof(EnvironmentContext))]
    [Migration("20230215092455_SplitDataSourceFromDataRecord")]
    partial class SplitDataSourceFromDataRecord
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("environmentMonitor.Data.DataRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("DataSourceId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("When")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceId");

                    b.HasIndex("When");

                    b.ToTable("DataRecords");
                });

            modelBuilder.Entity("environmentMonitor.Data.DataSource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MachineId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MachineId", "Key");

                    b.ToTable("DataSources");
                });

            modelBuilder.Entity("environmentMonitor.Data.DataRecord", b =>
                {
                    b.HasOne("environmentMonitor.Data.DataSource", null)
                        .WithMany()
                        .HasForeignKey("DataSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
