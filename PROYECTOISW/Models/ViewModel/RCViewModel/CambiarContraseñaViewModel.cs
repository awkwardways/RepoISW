using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModels.RCViewModels
{
    public class CambiarContraseñaViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string? Nueva { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Nueva", ErrorMessage = "Las contraseñas no coinciden")]
        public string? Confirmar { get; set; }
        public string? Correo { get; set; }
        public string? Token { get; set; }
    }
}
