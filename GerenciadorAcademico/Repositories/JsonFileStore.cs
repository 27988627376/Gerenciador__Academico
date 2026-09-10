using System.Text.Json;

namespace GerenciadorAcademico.Repositories
{
    public class JsonFileStore
    {
        private static readonly SemaphoreSlim EscritaLock = new(1, 1);
        private static readonly JsonSerializerOptions OpcoesJson = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private readonly string _diretorioDados;

        public JsonFileStore(IWebHostEnvironment environment)
        {
            _diretorioDados = Path.Combine(environment.ContentRootPath, "Data");
        }

        public Task<List<T>> LerAsync<T>(string nomeArquivo) => LerInternoAsync<T>(ObterCaminho(nomeArquivo));

        public async Task<TResult> AlterarAsync<T, TResult>(string nomeArquivo, Func<List<T>, TResult> alteracao)
        {
            var caminho = ObterCaminho(nomeArquivo);
            await EscritaLock.WaitAsync();

            try
            {
                var itens = await LerInternoAsync<T>(caminho);
                var resultado = alteracao(itens);
                await GravarAtomicoAsync(caminho, itens);
                return resultado;
            }
            finally
            {
                EscritaLock.Release();
            }
        }

        private string ObterCaminho(string nomeArquivo)
        {
            if (Path.GetFileName(nomeArquivo) != nomeArquivo)
                throw new ArgumentException("O nome do arquivo JSON é inválido.", nameof(nomeArquivo));

            return Path.Combine(_diretorioDados, nomeArquivo);
        }

        private static async Task<List<T>> LerInternoAsync<T>(string caminho)
        {
            if (!File.Exists(caminho))
                throw new FileNotFoundException("O arquivo de dados não foi encontrado.", caminho);

            var json = await File.ReadAllTextAsync(caminho);
            if (string.IsNullOrWhiteSpace(json))
                return [];

            return JsonSerializer.Deserialize<List<T>>(json, OpcoesJson) ?? [];
        }

        private static async Task GravarAtomicoAsync<T>(string caminho, List<T> itens)
        {
            var temporario = $"{caminho}.{Guid.NewGuid():N}.tmp";

            try
            {
                await File.WriteAllTextAsync(temporario, JsonSerializer.Serialize(itens, OpcoesJson));
                File.Move(temporario, caminho, true);
            }
            finally
            {
                if (File.Exists(temporario))
                    File.Delete(temporario);
            }
        }
    }
}