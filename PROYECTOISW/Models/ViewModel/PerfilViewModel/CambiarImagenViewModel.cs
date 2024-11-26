using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.PerfilViewModel
{
    public class CambiarImagenViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Nueva Imagen")]
        public byte[]? Imagen { get; set; }
    }
}
