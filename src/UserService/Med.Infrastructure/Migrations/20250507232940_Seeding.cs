using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Med.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Specialities",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("2c9d2f77-6c1e-4b47-83f0-f2d1ab3147f5"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dermatologia" },
                    { new Guid("3f4a6e72-e22c-4ae0-ae7e-7201c6ddc9c2"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pediatria" },
                    { new Guid("4a4c0f8e-b28b-4e1b-a4ea-5a4b79fd9b6c"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardiologia" },
                    { new Guid("5811db18-b55b-4f34-95cf-11f3f0f89193"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ginecologia e Obstetrícia" },
                    { new Guid("79c5e160-9de3-446a-b44c-9b6a982cc4f0"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neurologia" },
                    { new Guid("87d7a295-0a4e-4c88-9367-0d83a746e6a7"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Endocrinologia" },
                    { new Guid("9d3b5c38-61c4-4bc6-b9ea-3acdb148c1ee"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ortopedia" },
                    { new Guid("a8e05346-29e6-4f30-94e2-e4ecf77b0d4b"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Urologia" },
                    { new Guid("b2bc226c-96d3-401f-88e2-70e62f66ae8f"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gastroenterologia" },
                    { new Guid("fde82618-bb0d-4a27-b232-3d3c204f00b4"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Psiquiatria" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("2c9d2f77-6c1e-4b47-83f0-f2d1ab3147f5"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("3f4a6e72-e22c-4ae0-ae7e-7201c6ddc9c2"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("4a4c0f8e-b28b-4e1b-a4ea-5a4b79fd9b6c"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("5811db18-b55b-4f34-95cf-11f3f0f89193"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("79c5e160-9de3-446a-b44c-9b6a982cc4f0"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("87d7a295-0a4e-4c88-9367-0d83a746e6a7"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("9d3b5c38-61c4-4bc6-b9ea-3acdb148c1ee"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("a8e05346-29e6-4f30-94e2-e4ecf77b0d4b"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("b2bc226c-96d3-401f-88e2-70e62f66ae8f"));

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "Id",
                keyValue: new Guid("fde82618-bb0d-4a27-b232-3d3c204f00b4"));
        }
    }
}
