using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amandaba.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TAB_ESPECIE",
                columns: table => new
                {
                    ID_ESPECIE = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_ESPECIE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_ESPECIE", x => x.ID_ESPECIE);
                });

            migrationBuilder.CreateTable(
                name: "TAB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    NR_CPF_USUARIO = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    DT_NASCIMENTO_USUARIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DS_EMAIL_USUARIO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    NR_TELEFONE_USUARIO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DS_SENHA_HASH_USUARIO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DT_CADASTRO_USUARIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ST_USUARIO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TAB_VACINA",
                columns: table => new
                {
                    ID_VACINA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_VACINA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    TP_VACINA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    NM_FABRICANTE_VACINA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    DS_VACINA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_VACINA", x => x.ID_VACINA);
                });

            migrationBuilder.CreateTable(
                name: "TAB_TUTOR",
                columns: table => new
                {
                    ID_TUTOR = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO_TUTOR = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_TUTOR", x => x.ID_TUTOR);
                    table.ForeignKey(
                        name: "FK_TAB_TUTOR_TAB_USUARIO_ID_USUARIO_TUTOR",
                        column: x => x.ID_USUARIO_TUTOR,
                        principalTable: "TAB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET",
                columns: table => new
                {
                    ID_PET = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_TUTOR_PET = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    ID_ESPECIE_PET = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    NM_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_FOTO_PET = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    NM_RACA_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    TP_SEXO_PET = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    DT_NASCIMENTO_PET = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NM_COR_PET = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    FL_CASTRADO_PET = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    NR_MICROCHIP_PET = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DT_CADASTRO_PET = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ST_PET = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET", x => x.ID_PET);
                    table.ForeignKey(
                        name: "FK_TAB_PET_TAB_ESPECIE_ID_ESPECIE_PET",
                        column: x => x.ID_ESPECIE_PET,
                        principalTable: "TAB_ESPECIE",
                        principalColumn: "ID_ESPECIE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TAB_PET_TAB_TUTOR_ID_TUTOR_PET",
                        column: x => x.ID_TUTOR_PET,
                        principalTable: "TAB_TUTOR",
                        principalColumn: "ID_TUTOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_CONSULTA",
                columns: table => new
                {
                    ID_CONSULTA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_CONSULTA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    DT_CONSULTA_PET = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DS_HORARIO_CONSULTA = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: true),
                    NM_VETERINARIO_CONSULTA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    NM_CLINICA_CONSULTA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    DS_MOTIVO_CONSULTA = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DS_SINTOMAS_CONSULTA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    VL_PESO_CONSULTA = table.Column<decimal>(type: "DECIMAL(6,2)", precision: 6, scale: 2, nullable: true),
                    DS_DIAGNOSTICO_CONSULTA = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DS_TRATAMENTO_CONSULTA = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DS_OBSERVACAO_CONSULTA = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    FL_RETORNO_CONSULTA = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    DT_RETORNO_CONSULTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ST_CONSULTA = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DT_CADASTRO_CONSULTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_CONSULTA", x => x.ID_CONSULTA);
                    table.ForeignKey(
                        name: "FK_TAB_CONSULTA_TAB_PET_ID_PET_CONSULTA",
                        column: x => x.ID_PET_CONSULTA,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_EXAME",
                columns: table => new
                {
                    ID_EXAME = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_EXAME = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    NM_EXAME_PET = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DT_SOLICITACAO_EXAME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DT_REALIZACAO_EXAME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NM_VETERINARIO_EXAME = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    NM_CLINICA_EXAME = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    DS_MOTIVO_EXAME = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DS_RESULTADO_EXAME = table.Column<string>(type: "NCLOB", maxLength: 4000, nullable: true),
                    DS_OBSERVACAO_EXAME = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DS_ARQUIVO_EXAME = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    ST_EXAME = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DT_CADASTRO_EXAME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_EXAME", x => x.ID_EXAME);
                    table.ForeignKey(
                        name: "FK_TAB_EXAME_TAB_PET_ID_PET_EXAME",
                        column: x => x.ID_PET_EXAME,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET_ALERGIA",
                columns: table => new
                {
                    ID_REGISTRO_ALERGIA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_ALERGIA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    NM_ALERGIA_PET = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    TP_ALERGIA_PET = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    DT_IDENTIFICACAO_ALERGIA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DS_REACAO_ALERGIA = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    TP_GRAVIDADE_ALERGIA = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DS_OBSERVACAO_ALERGIA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DT_CADASTRO_ALERGIA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET_ALERGIA", x => x.ID_REGISTRO_ALERGIA);
                    table.ForeignKey(
                        name: "FK_TAB_PET_ALERGIA_TAB_PET_ID_PET_ALERGIA",
                        column: x => x.ID_PET_ALERGIA,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET_DOENCA",
                columns: table => new
                {
                    ID_REGISTRO_DOENCA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_DOENCA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    NM_DOENCA_PET = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    DT_DIAGNOSTICO_DOENCA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ST_DOENCA_PET = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DS_TRATAMENTO_DOENCA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DS_OBSERVACAO_DOENCA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DT_CADASTRO_DOENCA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET_DOENCA", x => x.ID_REGISTRO_DOENCA);
                    table.ForeignKey(
                        name: "FK_TAB_PET_DOENCA_TAB_PET_ID_PET_DOENCA",
                        column: x => x.ID_PET_DOENCA,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET_MEDICAMENTO",
                columns: table => new
                {
                    ID_REGISTRO_MEDICAMENTO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_MEDICAMENTO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    NM_MEDICAMENTO_PET = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    DS_MOTIVO_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    VL_DOSAGEM_MEDICAMENTO = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: true),
                    TP_UNIDADE_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    QT_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DS_FREQUENCIA_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DS_ADMINISTRACAO_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DS_HORARIO_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DT_INICIO_MEDICAMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_TERMINO_MEDICAMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ST_MEDICAMENTO_PET = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DS_PRESCRICAO_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DS_OBSERVACAO_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DT_CADASTRO_MEDICAMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET_MEDICAMENTO", x => x.ID_REGISTRO_MEDICAMENTO);
                    table.ForeignKey(
                        name: "FK_TAB_PET_MEDICAMENTO_TAB_PET_ID_PET_MEDICAMENTO",
                        column: x => x.ID_PET_MEDICAMENTO,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET_PESO",
                columns: table => new
                {
                    ID_HISTORICO_PESO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_PESO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    VL_PESO_PET = table.Column<decimal>(type: "DECIMAL(6,2)", precision: 6, scale: 2, nullable: false),
                    DT_MEDICAO_PESO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_CADASTRO_PESO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET_PESO", x => x.ID_HISTORICO_PESO);
                    table.ForeignKey(
                        name: "FK_TAB_PET_PESO_TAB_PET_ID_PET_PESO",
                        column: x => x.ID_PET_PESO,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAB_PET_VACINA",
                columns: table => new
                {
                    ID_APLICACAO_VACINA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET_VACINA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    ID_VACINA_APLICADA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    DT_APLICACAO_VACINA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NR_DOSE_VACINA = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    NR_LOTE_VACINA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DT_PROXIMA_DOSE_VACINA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NM_CLINICA_VACINA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    DS_OBSERVACAO_VACINA = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DS_COMPROVANTE_VACINA = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DT_CADASTRO_APLICACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_PET_VACINA", x => x.ID_APLICACAO_VACINA);
                    table.ForeignKey(
                        name: "FK_TAB_PET_VACINA_TAB_PET_ID_PET_VACINA",
                        column: x => x.ID_PET_VACINA,
                        principalTable: "TAB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TAB_PET_VACINA_TAB_VACINA_ID_VACINA_APLICADA",
                        column: x => x.ID_VACINA_APLICADA,
                        principalTable: "TAB_VACINA",
                        principalColumn: "ID_VACINA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TAB_CONSULTA_ID_PET_CONSULTA",
                table: "TAB_CONSULTA",
                column: "ID_PET_CONSULTA");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_ESPECIE_NM_ESPECIE",
                table: "TAB_ESPECIE",
                column: "NM_ESPECIE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TAB_EXAME_ID_PET_EXAME",
                table: "TAB_EXAME",
                column: "ID_PET_EXAME");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_ID_ESPECIE_PET",
                table: "TAB_PET",
                column: "ID_ESPECIE_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_ID_TUTOR_PET",
                table: "TAB_PET",
                column: "ID_TUTOR_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_ALERGIA_ID_PET_ALERGIA",
                table: "TAB_PET_ALERGIA",
                column: "ID_PET_ALERGIA");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_DOENCA_ID_PET_DOENCA",
                table: "TAB_PET_DOENCA",
                column: "ID_PET_DOENCA");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_MEDICAMENTO_ID_PET_MEDICAMENTO",
                table: "TAB_PET_MEDICAMENTO",
                column: "ID_PET_MEDICAMENTO");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_PESO_ID_PET_PESO",
                table: "TAB_PET_PESO",
                column: "ID_PET_PESO");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_VACINA_ID_PET_VACINA",
                table: "TAB_PET_VACINA",
                column: "ID_PET_VACINA");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_PET_VACINA_ID_VACINA_APLICADA",
                table: "TAB_PET_VACINA",
                column: "ID_VACINA_APLICADA");

            migrationBuilder.CreateIndex(
                name: "IX_TAB_TUTOR_ID_USUARIO_TUTOR",
                table: "TAB_TUTOR",
                column: "ID_USUARIO_TUTOR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TAB_USUARIO_DS_EMAIL_USUARIO",
                table: "TAB_USUARIO",
                column: "DS_EMAIL_USUARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TAB_USUARIO_NR_CPF_USUARIO",
                table: "TAB_USUARIO",
                column: "NR_CPF_USUARIO",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAB_CONSULTA");

            migrationBuilder.DropTable(
                name: "TAB_EXAME");

            migrationBuilder.DropTable(
                name: "TAB_PET_ALERGIA");

            migrationBuilder.DropTable(
                name: "TAB_PET_DOENCA");

            migrationBuilder.DropTable(
                name: "TAB_PET_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "TAB_PET_PESO");

            migrationBuilder.DropTable(
                name: "TAB_PET_VACINA");

            migrationBuilder.DropTable(
                name: "TAB_PET");

            migrationBuilder.DropTable(
                name: "TAB_VACINA");

            migrationBuilder.DropTable(
                name: "TAB_ESPECIE");

            migrationBuilder.DropTable(
                name: "TAB_TUTOR");

            migrationBuilder.DropTable(
                name: "TAB_USUARIO");
        }
    }
}
