using GerenciadorAcademico.Services;
using GerenciadorAcademico.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorAcademico.Controllers
{
    public class ProjetosController : Controller
    {
        private readonly IProjetoService _projetoService;

        public ProjetosController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        public async Task<IActionResult> Index(string? pesquisa, string? ordenarPor)
        {
            var projetos = await _projetoService.PesquisarPorTituloAsync(pesquisa);
            projetos = _projetoService.Ordenar(projetos, ordenarPor);

            var model = new ProjetosIndexViewModel
            {
                Projetos = projetos,
                TextoPesquisa = pesquisa,
                QuantidadeTotal = projetos.Count,
                OrdenarPor = ordenarPor
            };

            return View(model);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var projeto = await _projetoService.ObterPorIdAsync(id);

            if (projeto is null)
                return NotFound();

            return View(projeto);
        }

        // =========================
        // CADASTRAR
        // =========================

        [HttpGet]
        public async Task<IActionResult> Cadastrar()
        {
            var model = new NovoProjetoViewModel
            {
                Professores = await ObterProfessoresSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(NovoProjetoViewModel model)
        {
            model.Professores = await ObterProfessoresSelectListAsync();

            if (!ModelState.IsValid)
                return View(model);

            await _projetoService.AdicionarAsync(model);
            TempData["Mensagem"] = "Projeto cadastrado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDITAR
        // =========================

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var projeto = await _projetoService.ObterPorIdAsync(id);

            if (projeto is null)
                return NotFound();

            var model = new EditarProjetoViewModel
            {
                Id = projeto.Id,
                Titulo = projeto.Titulo,
                Descricao = projeto.Descricao,
                CargaHoraria = projeto.CargaHoraria,
                ProfessorId = projeto.ProfessorId,
                Professores = await ObterProfessoresSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarProjetoViewModel model)
        {
            model.Professores = await ObterProfessoresSelectListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var atualizado = await _projetoService.AtualizarAsync(model);

            if (!atualizado)
                return NotFound();

            TempData["Mensagem"] = "Projeto atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EXCLUIR
        // =========================

        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var projeto = await _projetoService.ObterPorIdAsync(id);

            if (projeto is null)
                return NotFound();

            return View(projeto);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var removido = await _projetoService.RemoverAsync(id);

            if (!removido)
                return NotFound();

            TempData["Mensagem"] = "Projeto excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // PROFESSORES
        // =========================

        private async Task<List<SelectListItem>> ObterProfessoresSelectListAsync()
        {
            return (await _projetoService.ListarProfessoresAsync())
                .Select(professor => new SelectListItem
                {
                    Value = professor.Id.ToString(),
                    Text = professor.Nome
                })
                .ToList();
        }
    }
}