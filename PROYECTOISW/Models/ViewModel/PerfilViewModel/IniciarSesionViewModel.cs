using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class IniciarSesionViewModel
    {
        public int? Id { get; set; }

        public string? Tipo { get; set; }

        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Correo invalido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        public string? Contraseña { get; set; }

        public string? Telefono { get; set; }

        public byte[]? Foto { get; set; }
    }
}
