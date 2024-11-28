using System.ComponentModel.DataAnnotations;

namespace PROYECTOISW.Models.ViewModel.ComentariosViewModel
{
    public class ComentarioViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        public string? Comentario { get; set; }
        public int Calificacion { get; set; }

        public int IdPropiedad { get; set; }
    }
}
