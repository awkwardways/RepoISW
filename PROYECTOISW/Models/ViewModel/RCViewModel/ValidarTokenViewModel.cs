using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModels.RCViewModels
{
    public class ValidarTokenViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Ingresa el código")]
        public string? Token { get; set; }
        
        public string? Correo { get; set; }
    }
}
