using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class CrearUsuarioViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Tipo de usuario")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Correo Electronico")]
        [EmailAddress(ErrorMessage = "Correo Invalido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden")]
        public string RContraseña { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(10, ErrorMessage = "El numero no debe exceder los 10 digitos")]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fotos de la propiedad")]
        public IFormFile Foto { get; set; }

        public byte[]? Data { get; set; }
    }
}
