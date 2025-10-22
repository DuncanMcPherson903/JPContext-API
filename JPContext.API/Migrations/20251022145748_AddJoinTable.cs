using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JPContext.API.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleVocabulary");

            migrationBuilder.CreateTable(
                name: "ExampleVocabularies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExampleId = table.Column<int>(type: "integer", nullable: false),
                    VocabularyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleVocabularies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleVocabularies_Examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExampleVocabularies_Vocabulary_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabulary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleVocabularies_ExampleId",
                table: "ExampleVocabularies",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleVocabularies_VocabularyId",
                table: "ExampleVocabularies",
                column: "VocabularyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleVocabularies");

            migrationBuilder.CreateTable(
                name: "ExampleVocabulary",
                columns: table => new
                {
                    ExamplesId = table.Column<int>(type: "integer", nullable: false),
                    VocabularyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleVocabulary", x => new { x.ExamplesId, x.VocabularyId });
                    table.ForeignKey(
                        name: "FK_ExampleVocabulary_Examples_ExamplesId",
                        column: x => x.ExamplesId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExampleVocabulary_Vocabulary_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabulary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleVocabulary_VocabularyId",
                table: "ExampleVocabulary",
                column: "VocabularyId");
        }
    }
}
