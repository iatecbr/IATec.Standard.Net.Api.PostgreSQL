using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "people");

            migrationBuilder.CreateTable(
                name: "person",
                schema: "people",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name_value = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    middle_name_value = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    last_name_value = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    external_id = table.Column<string>(type: "char(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_person", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document",
                schema: "people",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    value_value = table.Column<string>(type: "varchar", maxLength: 150, nullable: false),
                    issuer_value = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    external_id = table.Column<string>(type: "char(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_document", x => x.id);
                    table.ForeignKey(
                        name: "fk_document_person_person_id",
                        column: x => x.person_id,
                        principalSchema: "people",
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_document_person_id",
                schema: "people",
                table: "document",
                column: "person_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document",
                schema: "people");

            migrationBuilder.DropTable(
                name: "person",
                schema: "people");
        }
    }
}
