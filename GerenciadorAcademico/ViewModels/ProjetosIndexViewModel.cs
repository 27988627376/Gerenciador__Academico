using GerenciadorAcademico.Models;

namespace GerenciadorAcademico.ViewModels
{
    public class ProjetosIndexViewModel
    {
        public List<Projeto> Projetos { get; set; } = [];
        public string? TextoPesquisa { get; set; }
        public int QuantidadeTotal { get; set; }
        public string? OrdenarPor { get; set; }
    }
}