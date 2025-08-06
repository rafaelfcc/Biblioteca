using Biblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Biblioteca.Domain.Contracts;

namespace Biblioteca.Repositories.Services
{
    public class RelatorioPdf : IRelatorioPdf
    {
        public byte[] GerarRelatorioLivros(List<Livro> livros, string? usuarioEmail = null)
        {
            // Configurações globais do QuestPDF (opcional, mas recomendado para fontes)
            // Você pode registrar fontes aqui se precisar de fontes específicas no PDF
            // FontManager.RegisterSystemFonts(); // Registra todas as fontes do sistema
            // Ou registrar fontes específicas:
            // FontManager.RegisterFont(File.ReadAllBytes("caminho/para/sua/fonte.ttf"));

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial)); // Use Arial ou outra fonte padrão

                    page.Header()
                        .PaddingBottom(1, Unit.Centimetre)
                        .Column(headerCol =>
                        {
                            headerCol.Item().Text("Relatório de Livros")
                                .SemiBold().FontSize(24).AlignCenter().FontColor(Colors.Blue.Medium);

                            if (!string.IsNullOrEmpty(usuarioEmail))
                            {
                                headerCol.Item().PaddingTop(0.5f, Unit.Centimetre).Text($"Livros Cadastrados por: {usuarioEmail}")
                                    .FontSize(14).AlignCenter();
                            }

                            // CORREÇÃO AQUI: Aplique PaddingTop ao item antes de adicionar o texto
                            headerCol.Item().PaddingTop(0.5f, Unit.Centimetre).Text($"Data de Geração: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(10).AlignRight();
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            if (livros == null || !livros.Any())
                            {
                                column.Item().Text("Nenhum livro encontrado para os critérios selecionados.")
                                    .Italic().AlignCenter().FontSize(12).FontColor(Colors.Grey.Medium);
                            }
                            else
                            {
                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3); // Título
                                        columns.RelativeColumn(2); // Autor
                                        columns.RelativeColumn(1.5f); // ISBN
                                        columns.RelativeColumn(1.5f); // Gênero
                                        columns.RelativeColumn(1.5f); // Editora
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Título").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Autor").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("ISBN").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Gênero").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Editora").SemiBold();
                                    });

                                    foreach (var livro in livros)
                                    {
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(livro.Titulo);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(livro.Autor);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(livro.ISBN);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(livro.GeneroLivro?.Genero ?? "N/A");
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(livro.Editora?.Nome ?? "N/A");
                                    }
                                });
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ").FontSize(9);
                            x.CurrentPageNumber().FontSize(9);
                            x.Span(" de ").FontSize(9);
                            x.TotalPages().FontSize(9);
                        });
                });
            }).GeneratePdf();
        }
    }
}
