namespace GerenciadorAcademico.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int CargaHoraria { get; set; }
        public int ProfessorId { get; set; }
        public Professor? Professor { get; set; }
    }
}