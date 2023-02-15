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
    [Migration("20230215091519_DataRecordWhenIndex")]
    partial class DataRecordWhenIndex
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

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MachineId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("When")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("When");

                    b.ToTable("DataRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
