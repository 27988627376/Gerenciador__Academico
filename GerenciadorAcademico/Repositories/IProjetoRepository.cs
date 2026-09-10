using GerenciadorAcademico.Models;

namespace GerenciadorAcademico.Repositories
{
    public interface IProjetoRepository
    {
        Task<List<Projeto>> ListarAsync();
        Task<Projeto?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Projeto projeto);
        Task<bool> AtualizarAsync(Projeto projeto);
        Task<bool> RemoverAsync(int id);
    }
}