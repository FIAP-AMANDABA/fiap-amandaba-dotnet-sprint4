using Amandaba.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace Amandaba.API.Infrastructure.Data.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20261020000000_AddVeterinariosCompartilhamentos")]
public partial class AddVeterinariosCompartilhamentos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TAB_VETERINARIO",
            columns: table => new
            {
                ID_VETERINARIO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                    .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                ID_USUARIO_VETERINARIO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                NR_CRMV_VETERINARIO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                ST_VETERINARIO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TAB_VETERINARIO", x => x.ID_VETERINARIO);
                table.ForeignKey("FK_USUARIO_VETERINARIO", x => x.ID_USUARIO_VETERINARIO,
                    "TAB_USUARIO", "ID_USUARIO", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateIndex("UK_USUARIO_VETERINARIO", "TAB_VETERINARIO", "ID_USUARIO_VETERINARIO", unique: true);
        migrationBuilder.CreateIndex("UK_CRMV_VETERINARIO", "TAB_VETERINARIO", "NR_CRMV_VETERINARIO", unique: true);

        migrationBuilder.CreateTable(
            name: "TAB_PET_VETERINARIO",
            columns: table => new
            {
                ID_PET_VETERINARIO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                    .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                ID_PET_COMPARTILHADO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                ID_VETERINARIO_ACESSO = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                DT_COMPARTILHAMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                ST_COMPARTILHAMENTO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TAB_PET_VETERINARIO", x => x.ID_PET_VETERINARIO);
                table.ForeignKey("FK_PET_COMPARTILHADO", x => x.ID_PET_COMPARTILHADO,
                    "TAB_PET", "ID_PET", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_VETERINARIO_ACESSO", x => x.ID_VETERINARIO_ACESSO,
                    "TAB_VETERINARIO", "ID_VETERINARIO", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateIndex("UK_PET_VETERINARIO", "TAB_PET_VETERINARIO",
            new[] { "ID_PET_COMPARTILHADO", "ID_VETERINARIO_ACESSO" }, unique: true);
        migrationBuilder.CreateIndex("IX_PET_VETERINARIO_VET", "TAB_PET_VETERINARIO", "ID_VETERINARIO_ACESSO");
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("TAB_PET_VETERINARIO");
        migrationBuilder.DropTable("TAB_VETERINARIO");
    }
}
