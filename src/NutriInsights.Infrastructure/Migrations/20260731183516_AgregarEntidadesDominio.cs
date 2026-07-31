using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriInsights.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEntidadesDominio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Estatura",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaNacimiento",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NivelActividad",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Peso",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoriasAlimento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    PorcionReferenciaGramos = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasAlimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objetivos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nutriente = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objetivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objetivos_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Comida = table.Column<int>(type: "integer", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registros_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesMedida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    CategoriaAlimentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Origen = table.Column<int>(type: "integer", nullable: false),
                    UsuarioPropietarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    CodigoExterno = table.Column<string>(type: "text", nullable: true),
                    CaloriasPor100g = table.Column<decimal>(type: "numeric", nullable: true),
                    ProteinaPor100g = table.Column<decimal>(type: "numeric", nullable: true),
                    CarbohidratosPor100g = table.Column<decimal>(type: "numeric", nullable: true),
                    GrasaPor100g = table.Column<decimal>(type: "numeric", nullable: true),
                    FibraPor100g = table.Column<decimal>(type: "numeric", nullable: true),
                    NivelConfianza = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alimentos_AspNetUsers_UsuarioPropietarioId",
                        column: x => x.UsuarioPropietarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alimentos_CategoriasAlimento_CategoriaAlimentoId",
                        column: x => x.CategoriaAlimentoId,
                        principalTable: "CategoriasAlimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalificadoresCantidad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaAlimentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Calificador = table.Column<int>(type: "integer", nullable: false),
                    MinGramos = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxGramos = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalificadoresCantidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalificadoresCantidad_CategoriasAlimento_CategoriaAlimentoId",
                        column: x => x.CategoriaAlimentoId,
                        principalTable: "CategoriasAlimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlimentosUnidadEquivalencia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlimentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnidadMedidaId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquivalenteEnGramos = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlimentosUnidadEquivalencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlimentosUnidadEquivalencia_Alimentos_AlimentoId",
                        column: x => x.AlimentoId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlimentosUnidadEquivalencia_UnidadesMedida_UnidadMedidaId",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsDeRegistro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistroId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlimentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    DescripcionLibre = table.Column<string>(type: "text", nullable: true),
                    Cantidad = table.Column<decimal>(type: "numeric", nullable: true),
                    UnidadMedidaId = table.Column<Guid>(type: "uuid", nullable: true),
                    FraccionAplicada = table.Column<decimal>(type: "numeric", nullable: true),
                    NivelEstimacion = table.Column<int>(type: "integer", nullable: false),
                    ValorCaloriasManual = table.Column<decimal>(type: "numeric", nullable: true),
                    CaloriasSnapshot = table.Column<decimal>(type: "numeric", nullable: true),
                    ProteinaSnapshot = table.Column<decimal>(type: "numeric", nullable: true),
                    CarbohidratosSnapshot = table.Column<decimal>(type: "numeric", nullable: true),
                    GrasaSnapshot = table.Column<decimal>(type: "numeric", nullable: true),
                    FibraSnapshot = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsDeRegistro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsDeRegistro_Alimentos_AlimentoId",
                        column: x => x.AlimentoId,
                        principalTable: "Alimentos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemsDeRegistro_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsDeRegistro_UnidadesMedida_UnidadMedidaId",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alimentos_CategoriaAlimentoId",
                table: "Alimentos",
                column: "CategoriaAlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alimentos_UsuarioPropietarioId",
                table: "Alimentos",
                column: "UsuarioPropietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentosUnidadEquivalencia_AlimentoId",
                table: "AlimentosUnidadEquivalencia",
                column: "AlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentosUnidadEquivalencia_UnidadMedidaId",
                table: "AlimentosUnidadEquivalencia",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_CalificadoresCantidad_CategoriaAlimentoId",
                table: "CalificadoresCantidad",
                column: "CategoriaAlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDeRegistro_AlimentoId",
                table: "ItemsDeRegistro",
                column: "AlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDeRegistro_RegistroId",
                table: "ItemsDeRegistro",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDeRegistro_UnidadMedidaId",
                table: "ItemsDeRegistro",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_UsuarioId",
                table: "Objetivos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_UsuarioId",
                table: "Registros",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlimentosUnidadEquivalencia");

            migrationBuilder.DropTable(
                name: "CalificadoresCantidad");

            migrationBuilder.DropTable(
                name: "ItemsDeRegistro");

            migrationBuilder.DropTable(
                name: "Objetivos");

            migrationBuilder.DropTable(
                name: "Alimentos");

            migrationBuilder.DropTable(
                name: "Registros");

            migrationBuilder.DropTable(
                name: "UnidadesMedida");

            migrationBuilder.DropTable(
                name: "CategoriasAlimento");

            migrationBuilder.DropColumn(
                name: "Estatura",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NivelActividad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Peso",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "AspNetUsers");
        }
    }
}
