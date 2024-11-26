using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class CambiarContraseñaViewModel
    {
        public int? Id { get; set; }

        public string? Correo { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña Actual")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        public string? Actual { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Nueva Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string? NuevaContraseña { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarContraseña { get; set; }
    }
}
