using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorAcademico.ViewModels;

public class ProjetoFormularioViewModel
{
    [Display(Name = "Título")]
    [Required(ErrorMessage = "Informe o título do projeto.")]
    [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "Informe a descrição do projeto.")]
    [MinLength(20, ErrorMessage = "A descrição deve ter pelo menos 20 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Carga horária")]
    [Range(1, 1000, ErrorMessage = "A carga horária deve estar entre 1 e 1000 horas.")]
    public int CargaHoraria { get; set; }

    [Display(Name = "Professor")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um professor.")]
    public int ProfessorId { get; set; }

    public List<SelectListItem> Professores { get; set; } = [];
}