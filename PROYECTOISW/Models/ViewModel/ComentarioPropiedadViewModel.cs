using PROYECTOISW.Models.ViewModel.ComentariosViewModel;

namespace PROYECTOISW.Models.ViewModel
{
    public class ComentarioPropiedadViewModel
    {
        public List<Propiedade> Propiedad { get; set; }
        public ComentarioViewModel Comentario { get; set; }
        public List<bool> reseñas { get; set; }
    }
}
