using System.ComponentModel.DataAnnotations;

namespace CineReview.DTOs
{
    public class CriarAdministradorDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; }
    }

    public class AdministradorRespostaDTO
    {
        public Guid Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; } = "Administrador";
    }

    public class AtualizarAdministradorDTO
    {
        [Required] public string NomeUsuario { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
    }
}