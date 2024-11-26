using Microsoft.AspNetCore.Mvc.Rendering;

namespace PROYECTOISW.Models.ViewModel.PropiedadViewModel
{
    public class BusquedaViewModel
    {
        public int? MaxPrecio { get; set; }  //Filtro de precio maximo
        public int? MinPrecio { get; set; }  //Filtro de precio minimo
        public string? TipoInmueble { get; set; }   //filtro de tipo de propiedad
        public int? DistanciaAEscuela { get; set; }
        public List<(byte[] rawImagen, Propiedade propiedad, string mimeType)> ListaDePropiedades { get; set; }  //Lista de propiedades a desplegar
    }
}
