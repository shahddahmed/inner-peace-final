using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnerPeace.Migrations
{
    /// <inheritdoc />
    public partial class EducationDegree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Educations");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationDegreeId",
                table: "Educations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EducationDegree",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Degree = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDegree", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_EducationDegreeId",
                table: "Educations",
                column: "EducationDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_EducationDegree_EducationDegreeId",
                table: "Educations",
                column: "EducationDegreeId",
                principalTable: "EducationDegree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_EducationDegree_EducationDegreeId",
                table: "Educations");

            migrationBuilder.DropTable(
                name: "EducationDegree");

            migrationBuilder.DropIndex(
                name: "IX_Educations_EducationDegreeId",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "EducationDegreeId",
                table: "Educations");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Educations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
