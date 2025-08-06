using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Biblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Editora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editora", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneroLivro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroLivro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    EmailLogin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.EmailLogin);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EditoraId = table.Column<int>(type: "int", nullable: false),
                    Sinopse = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    UsuarioRegistro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CaminhoFoto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FotoLivro = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livros_Editora_EditoraId",
                        column: x => x.EditoraId,
                        principalTable: "Editora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livros_GeneroLivro_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "GeneroLivro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Editora",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Agir" },
                    { 2, "Martins Fontes" },
                    { 3, "Ricordi" },
                    { 4, "Benvirá" }
                });

            migrationBuilder.InsertData(
                table: "GeneroLivro",
                columns: new[] { "Id", "Genero" },
                values: new object[,]
                {
                    { 1, "Fábula" },
                    { 2, "Fantasia" },
                    { 3, "Didático" },
                    { 4, "Auto-Ajuda" },
                    { 5, "Romance" },
                    { 6, "Tecnologia" },
                    { 7, "Ciência" },
                    { 8, "Saúde e Bem-Estar" },
                    { 9, "Ciências Sociais" }
                });

            migrationBuilder.InsertData(
                table: "Livros",
                columns: new[] { "Id", "Autor", "CaminhoFoto", "EditoraId", "FotoLivro", "GeneroId", "ISBN", "Sinopse", "Titulo", "UsuarioRegistro" },
                values: new object[,]
                {
                    { new Guid("0fd11c8f-15ac-4e9a-abf2-7e2499fd2ff7"), "J.R.R. Tolkien", "0", 2, (byte)0, 2, "978-85-222-0573-5", "Uma épica jornada pela Terra Média em que um grupo improvável de heróis precisa destruir um anel mágico e maligno para evitar que um senhor das trevas domine o mundo.", "O Senhor dos Anéis", "rfccrj@gmail.com" },
                    { new Guid("1f098cac-9c52-4b63-9529-aaa0178083bc"), "Antoine de Saint-Exupéry", "0", 1, (byte)0, 1, "978-85-359-0277-7", "Uma história poética e filosófica contada por um piloto que encontra um pequeno príncipe vindo de outro planeta. A obra aborda temas como amizade, amor e o sentido da vida.", "O Pequeno Príncipe", "rfccrj@gmail.com" },
                    { new Guid("270abab8-6a79-4592-83f3-3070e146e259"), "Bohumil Med", "/uploads/2dfbf92a-77de-471d-3282-08ddd1ea4bf9_teoria_da_musica.JPG", 3, (byte)0, 3, "978-85-7060-013-1", "Obra consagrada que apresenta fundamentos teóricos da música com linguagem acessível e progressiva. Utilizada em escolas de música, conservatórios e cursos técnicos.", "Teoria da Música", "rfccrj@gmail.com" },
                    { new Guid("28ecc827-6eb3-498c-9583-95d560ff290e"), "Augusto Cury", "0", 4, (byte)0, 4, "978-85-452-0016-2", "O livro apresenta conceitos da psicologia para ajudar o leitor a entender e enfrentar a ansiedade, com foco na Síndrome do Pensamento Acelerado, termo cunhado pelo autor.", "Ansiedade - Como enfrentar o mal do século", "rfccrj@gmail.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_EditoraId",
                table: "Livros",
                column: "EditoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_GeneroId",
                table: "Livros",
                column: "GeneroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livros");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Editora");

            migrationBuilder.DropTable(
                name: "GeneroLivro");
        }
    }
}
