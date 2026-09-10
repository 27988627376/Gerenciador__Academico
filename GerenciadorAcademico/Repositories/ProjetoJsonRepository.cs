using GerenciadorAcademico.Models;

namespace GerenciadorAcademico.Repositories
{
    public class ProjetoJsonRepository : IProjetoRepository
    {
        private const string NomeArquivo = "projetos.json";
        private readonly JsonFileStore _arquivo;

        public ProjetoJsonRepository(JsonFileStore arquivo) => _arquivo = arquivo;

        public Task<List<Projeto>> ListarAsync() => _arquivo.LerAsync<Projeto>(NomeArquivo);

        public async Task<Projeto?> ObterPorIdAsync(int id) =>
            (await ListarAsync()).FirstOrDefault(projeto => projeto.Id == id);

        public async Task AdicionarAsync(Projeto projeto)
        {
            await _arquivo.AlterarAsync<Projeto, object?>(NomeArquivo, projetos =>
            {
                projeto.Id = projetos.Count == 0 ? 1 : projetos.Max(item => item.Id) + 1;
                projeto.Professor = null;
                projetos.Add(projeto);
                return null;
            });
        }

        public Task<bool> AtualizarAsync(Projeto projeto) => _arquivo.AlterarAsync<Projeto, bool>(NomeArquivo, projetos =>
        {
            var existente = projetos.FirstOrDefault(item => item.Id == projeto.Id);
            if (existente is null)
                return false;

            existente.Titulo = projeto.Titulo;
            existente.Descricao = projeto.Descricao;
            existente.CargaHoraria = projeto.CargaHoraria;
            existente.ProfessorId = projeto.ProfessorId;
            return true;
        });

        public Task<bool> RemoverAsync(int id) => _arquivo.AlterarAsync<Projeto, bool>(NomeArquivo, projetos =>
        {
            return projetos.RemoveAll(projeto => projeto.Id == id) > 0;
        });
    }
}
