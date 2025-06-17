using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnerPeace.Migrations
{
    /// <inheritdoc />
    public partial class FixDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Educations_EducationId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_EducationId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "EducationId",
                table: "Doctors");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "Educations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Educations_DoctorId",
                table: "Educations",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Doctors_DoctorId",
                table: "Educations",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Doctors_DoctorId",
                table: "Educations");

            migrationBuilder.DropIndex(
                name: "IX_Educations_DoctorId",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Educations");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationId",
                table: "Doctors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_EducationId",
                table: "Doctors",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Educations_EducationId",
                table: "Doctors",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
