namespace GerenciadorAcademico.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Projeto> Projetos { get; set; } = new List<Projeto>();
        
    }
}
