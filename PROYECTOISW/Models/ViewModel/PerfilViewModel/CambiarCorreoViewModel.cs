using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class CambiarCorreoViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType( DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Contraseña { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage = "Correo invalido")]
        [Display(Name = "Nuevo Correo Electronico")]
        public string? NuevoCorreo { get; set; }
    }
}
