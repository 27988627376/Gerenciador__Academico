using GerenciadorAcademico.Models;
using GerenciadorAcademico.Repositories;
using GerenciadorAcademico.ViewModels;

namespace GerenciadorAcademico.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IProfessorRepository _professorRepository;

        public ProjetoService(IProjetoRepository projetoRepository, IProfessorRepository professorRepository)
        {
            _projetoRepository = projetoRepository;
            _professorRepository = professorRepository;
        }

        public async Task<List<Professor>> ListarProfessoresAsync() => await _professorRepository.ListarAsync();

        public async Task<List<Projeto>> ListarAsync()
        {
            var projetos = await _projetoRepository.ListarAsync();
            return await VincularProfessoresAsync(projetos);
        }

        public async Task<List<Projeto>> PesquisarPorTituloAsync(string? titulo)
        {
            var projetos = await ListarAsync();
            if (string.IsNullOrWhiteSpace(titulo)) return projetos;

            return projetos.Where(projeto => projeto.Titulo.Contains(titulo, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public List<Projeto> Ordenar(IEnumerable<Projeto> projetos, string? ordenarPor) => ordenarPor?.ToLowerInvariant() switch
        {
            "titulo" => projetos.OrderBy(projeto => projeto.Titulo).ToList(),
            "cargahoraria" => projetos.OrderBy(projeto => projeto.CargaHoraria).ToList(),
            _ => projetos.ToList()
        };

        public async Task<Projeto?> ObterPorIdAsync(int id)
        {
            var projeto = await _projetoRepository.ObterPorIdAsync(id);
            if (projeto is not null)
                projeto.Professor = await _professorRepository.ObterPorIdAsync(projeto.ProfessorId);
            return projeto;
        }

        public async Task AdicionarAsync(NovoProjetoViewModel model)
        {
            if (await _professorRepository.ObterPorIdAsync(model.ProfessorId) is null)
                throw new InvalidOperationException("O professor selecionado não existe.");

            await _projetoRepository.AdicionarAsync(new Projeto
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                CargaHoraria = model.CargaHoraria,
                ProfessorId = model.ProfessorId
            });
        }

        public async Task<bool> AtualizarAsync(EditarProjetoViewModel model)
        {
            if (await _professorRepository.ObterPorIdAsync(model.ProfessorId) is null) return false;

            return await _projetoRepository.AtualizarAsync(new Projeto
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                CargaHoraria = model.CargaHoraria,
                ProfessorId = model.ProfessorId
            });
        }

        public Task<bool> RemoverAsync(int id) => _projetoRepository.RemoverAsync(id);

        private async Task<List<Projeto>> VincularProfessoresAsync(List<Projeto> projetos)
        {
            var professoresPorId = (await _professorRepository.ListarAsync()).ToDictionary(professor => professor.Id);
            foreach (var projeto in projetos)
                projeto.Professor = professoresPorId.GetValueOrDefault(projeto.ProfessorId);
            return projetos;
        }
    }
}