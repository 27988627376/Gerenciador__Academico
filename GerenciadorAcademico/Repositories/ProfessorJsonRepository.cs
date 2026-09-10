using GerenciadorAcademico.Models;

namespace GerenciadorAcademico.Repositories
{
    public class ProfessorJsonRepository : IProfessorRepository
    {
        private const string NomeArquivo = "professores.json";
        private readonly JsonFileStore _arquivo;

        public ProfessorJsonRepository(JsonFileStore arquivo) => _arquivo = arquivo;

        public Task<List<Professor>> ListarAsync() => _arquivo.LerAsync<Professor>(NomeArquivo);

        public async Task<Professor?> ObterPorIdAsync(int id) =>
            (await ListarAsync()).FirstOrDefault(professor => professor.Id == id);
    }
}