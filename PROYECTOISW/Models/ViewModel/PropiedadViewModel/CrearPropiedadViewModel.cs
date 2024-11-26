using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace PROYECTOISW.Models.ViewModel.PropiedadViewModel
{
    public class CrearPropiedadViewModel
    {
        //Identificar la propiedad por id
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima es de 50 letras")]
        [Display(Name = "Titulo de la publicación")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [MaxLength(600, ErrorMessage = "La longitud máxima es 600 letras")]
        [Display(Name = "Descripción del inmueble")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Tipo de inmueble")]
        public string? TipoPropiedad { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Range(1, 100000, ErrorMessage = "El precio es incorrecto")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        [Display(Name = "Precio de renta mensual")]
        public int PrecioRenta { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Range(1, 9999, ErrorMessage = "El rango maximo es 9999")]
        [Display(Name = "Superficie")]
        public string? Superficie { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Range(1, 9, ErrorMessage = "El máximo es 9")]
        [Display(Name = "Número de habitaciones")]
        public string? NumeroHabitaciones { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Range(1, 9, ErrorMessage = "El máximo es 9")]
        [Display(Name = "Número de baños con los que cuenta el inmueble")]
        public string? NumeroBaños { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima es 50 letras")]
        [Display(Name = "Servicios con los que cuenta el inmueble")]
        public string? Servicios { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [MaxLength(200, ErrorMessage = "La longitud máxima es 200 letras")]
        [Display(Name = "Dirección completa del inmueble")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Range(1, 999, ErrorMessage = "El máximo es 999 KM")]
        [Display(Name = "Distancia del inmueble a ESIME Culhuácan")]
        public int Distancia { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [MaxLength(200, ErrorMessage = "La longitud máxima es 200 letras")]
        [Display(Name = "Condiciones Especiales")]
        public string? CondicionesEspeciales { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fotos de la propiedad")]
        public List<IFormFile> archivosImagenes { get; set; }

        public List<byte[]>? Data { get; set; }
    }
}
