using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class CambiarTelefonoViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Password, ErrorMessage = "Error")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        public string? Contraseña {  get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Telefono no valido")]
        [MaxLength(10,ErrorMessage = "El telefono es a 10 digitos")]
        [Display(Name = "Nuevo Telefono")]
        public string? NuevoTelefono { get; set; }
    }
}
