using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.ContactarViewModel
{
    public class ContactarViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Duda")]
        public string? Duda { get; set; }
        public int IdPropiedad {  get; set; } 
    }
}
