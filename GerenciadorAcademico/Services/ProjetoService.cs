using GerenciadorAcademico.Models;
using GerenciadorAcademico.ViewModels;
using System.Collections.Generic;
using System.Linq;
using GerenciadorAcademico.Services;

namespace GerenciadorAcademico.Services
{
    public class ProjetoService : IProjetoService
    {
        private int _proximoId = 1;

        private readonly List<Professor> _professores =
[
    new Professor { Id = 1, Nome = "Ana Souza", Email = "ana.souza@academico.com" },
    new Professor { Id = 2, Nome = "Carlos Mendes", Email = "carlos.mendes@academico.com" },
    new Professor { Id = 3, Nome = "Beatriz Lima", Email = "beatriz.lima@academico.com" }
];

        private readonly List<Projeto> _projetos =
        [
            new Projeto
{
                Id = 1,
                Titulo = "Sistema de Biblioteca Escolar",
                Descricao = "Aplicação para controle de livros e empréstimos.",
                CargaHoraria = 80,
                ProfessorId = 1
            },
            new Projeto
            {
                Id = 2,
                Titulo = "Portal de Projetos Acadêmicos",
                Descricao = "Sistema para divulgação de projetos desenvolvidos pelos estudantes.",
                CargaHoraria = 60,
                ProfessorId = 2
            }

        ];

        public List<Professor> ListarProfessores()
        {
            return _professores;
        }
        public List<Projeto> Ordenar(IEnumerable<Projeto> projetos, string? ordenarPor)
        {
            return ordenarPor?.ToLowerInvariant() switch
            {
                "titulo" => projetos.OrderBy(projeto => projeto.Titulo).ToList(),
                "cargahoraria" => projetos.OrderBy(projeto => projeto.CargaHoraria).ToList(),
                _ => projetos.ToList()
            };
        }
        public List<Projeto> Listar()
        {
            return _projetos.Select(VincularProfessor).ToList();
        }

        private Professor? ObterProfessorPorId(int professorId)
        {
            return _professores.FirstOrDefault(professor => professor.Id == professorId);
        }

        private Projeto VincularProfessor(Projeto projeto)
        {
            projeto.Professor = ObterProfessorPorId(projeto.ProfessorId);
            return projeto;
        }

        public Projeto? ObterPorId(int id)
        {
            var projeto = _projetos.Select(VincularProfessor).FirstOrDefault(projeto => projeto.Id == id);

            if (projeto is null)
                return null;

            return VincularProfessor(projeto);
        }

        public void Adicionar(NovoProjetoViewModel model)
        {
            var novoProjeto = new Projeto
            {
                Id = GerarNovoId(),
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                CargaHoraria = model.CargaHoraria,
                ProfessorId = model.ProfessorId,
                Professor = ObterProfessorPorId(model.ProfessorId)
            };

            _projetos.Add(novoProjeto);
        }

        public bool Atualizar(EditarProjetoViewModel model)
        {
            var projeto = ObterPorId(model.Id);

            if (projeto is null)
                return false;

            projeto.Titulo = model.Titulo;
            projeto.Descricao = model.Descricao;
            projeto.CargaHoraria = model.CargaHoraria;
            projeto.ProfessorId = model.ProfessorId;
            projeto.Professor = ObterProfessorPorId(model.ProfessorId);
            return true;
        }

        
        public bool Remover(int id)
        {
            var projeto = ObterPorId(id);

            if (projeto is null)
                return false;
                
            _projetos.Remove(projeto);
            return true;
        }

        private int GerarNovoId()
        {
            return _projetos.Count == 0 ? 1 : _projetos.Max(projeto => projeto.Id) + 1;
        }

        public List<Projeto> PesquisarPorTitulo(string? titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return Listar();

            return _projetos
                .Where(projeto => projeto.Titulo.Contains(titulo, StringComparison.CurrentCultureIgnoreCase))
                .Select(VincularProfessor)
                .ToList();
        }
    }
}