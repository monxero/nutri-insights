using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NutriInsights.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSemillaCategoriasAlimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoriasAlimento",
                columns: new[] { "Id", "Nombre", "PorcionReferenciaGramos" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), "Proteína animal", 120m },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "Proteína vegetal / legumbres", 80m },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "Verduras", 100m },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "Frutas", 150m },
                    { new Guid("11111111-0000-0000-0000-000000000005"), "Carbohidratos / cereales", 150m },
                    { new Guid("11111111-0000-0000-0000-000000000006"), "Lácteos", 200m },
                    { new Guid("11111111-0000-0000-0000-000000000007"), "Grasas y frutos secos", 30m },
                    { new Guid("11111111-0000-0000-0000-000000000008"), "Otros / procesados", 50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "CategoriasAlimento",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000008"));
        }
    }
}
