﻿// <auto-generated />
using System;
using CityInfo.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityInfo.API.Migrations.DoctorManagement
{
    [DbContext(typeof(DoctorManagementContext))]
    [Migration("20240911100130_UpdateFluentAPI")]
    partial class UpdateFluentAPI
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CityInfo.API.DbContexts.Appointment", b =>
                {
                    b.Property<Guid>("AppoitmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppoitmentStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("AppoitmentTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppoitmentId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Patient", b =>
                {
                    b.Property<Guid>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Diseases")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PatientId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Patient", (string)null);
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PaymentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PaymentId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Appointment", b =>
                {
                    b.HasOne("CityInfo.API.DbContexts.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Patient_Appoitment");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Patient", b =>
                {
                    b.HasOne("CityInfo.API.DbContexts.Payment", "Payment")
                        .WithMany("Patients")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Patient_Payment");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Patient", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("CityInfo.API.DbContexts.Payment", b =>
                {
                    b.Navigation("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
