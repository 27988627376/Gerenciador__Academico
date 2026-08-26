using GerenciadorAcademico.Models;
using GerenciadorAcademico.ViewModels;

namespace GerenciadorAcademico.Services;
    public interface IProjetoService
        {
            List<Projeto> PesquisarPorTitulo(string? titulo);
            List<Projeto> Ordenar(IEnumerable<Projeto> projetos, string? ordenarPor);
            List<Projeto> Listar();
            Projeto? ObterPorId(int id);
            void Adicionar(NovoProjetoViewModel model);
            bool Atualizar(EditarProjetoViewModel model);
            bool Remover(int id);
            List<Professor> ListarProfessores();

}

