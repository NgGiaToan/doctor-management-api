using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations.DoctorManagement
{
    /// <inheritdoc />
    public partial class AddDoctorTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "AppoitmentTime",
                table: "Appointment",
                newName: "AppointmentTime");

            migrationBuilder.RenameColumn(
                name: "AppoitmentStatus",
                table: "Appointment",
                newName: "AppointmentStatus");

            migrationBuilder.RenameColumn(
                name: "AppoitmentId",
                table: "Appointment",
                newName: "DoctorId");

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "AppointmentId");

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Appoitment",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Appoitment",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Appointment",
                newName: "AppoitmentId");

            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Appointment",
                newName: "AppoitmentTime");

            migrationBuilder.RenameColumn(
                name: "AppointmentStatus",
                table: "Appointment",
                newName: "AppoitmentStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "AppoitmentId");
        }
    }
}
