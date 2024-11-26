using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModels.RCViewModels
{
    public class ValidarCorreoViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no es valido")]
        [Display(Name = "Correo Electrónico")]
        public string? Correo { get; set; }
    }
}
