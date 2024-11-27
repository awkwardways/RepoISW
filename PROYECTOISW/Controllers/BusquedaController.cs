using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Models;
using PROYECTOISW.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PROYECTOISW.Models.ViewModel.PropiedadViewModel;
using PROYECTOISW.Models.ViewModel;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace PROYECTOISW.Controllers
{
    [Authorize]
    public class BusquedaController : Controller
    {
        private readonly ProyectoiswContext _contexto; 
        public BusquedaController(ProyectoiswContext context) 
        {
            _contexto = context;
        }
        #region Busqueda
        [HttpGet]
        public async Task<IActionResult> Busqueda()
        {
            var publicaciones = await _contexto.Propiedades.Include(p => p.Imagenes).ToListAsync();
            var viewModel = new CompartidoViewModel();
            viewModel.Publicaciones = new List<Propiedade>();
            viewModel.Buscar = new BusquedaViewModel();

            viewModel.Publicaciones = publicaciones;
            return View(viewModel);
            #region Franco
            //var model = new BusquedaViewModel
            //{
            //    TiposDePropiedad = new List<SelectListItem>
            //    {
            //        new SelectListItem { Value = "Casa", Text = "Casa" },
            //        new SelectListItem { Value = "Departamento", Text = "Departamento" },
            //        new SelectListItem { Value = "Habitacion", Text = "Habitación"}
            //    },
            //    ListaDePropiedades = new List<(byte[] rawImagen, Propiedade propiedad, string mimeType)>()
            //};
            ////BusquedaViewModel viewModel = new BusquedaViewModel();
            ////List<(byte[] rawImagen, Propiedade propiedad, string TipoImagen)> cardsPropiedades = new List<(byte[] rawImagen, Propiedade propiedad, string TipoImagen)>();
            //var propiedades = await _contexto.Propiedades.ToListAsync();  // Recuperar todas las propiedades disponibles en la base de datos.
            //foreach (var currPropiedad in propiedades) 
            //{
            //    var primerImagenPropiedad = await _contexto.Imagenes.FirstOrDefaultAsync(f => f.IdPropiedad == currPropiedad.IdPropiedad);
            //    var mimeType = MimeTypeHelper.GetMimeType(primerImagenPropiedad.Imagen);
            //    model.ListaDePropiedades.Add((primerImagenPropiedad.Imagen, currPropiedad, mimeType));
            //}
            ////cardsPropiedades.
            ///
            #endregion
        }
        [HttpPost]
        public async Task<IActionResult> Busqueda(CompartidoViewModel model) 
        {
            //Agregar filtros
            var publicaciones = await _contexto.Propiedades
                    .Where(p => p.TipoPropiedad == model.Buscar.TipoInmueble &&
                    p.Distancia >= model.Buscar.DistanciaAEscuela &&
                    (p.PrecioRenta <= model.Buscar.MaxPrecio && p.PrecioRenta >= model.Buscar.MinPrecio))
                    .Include(p => p.Imagenes)
                    .ToListAsync();
            
            model.Publicaciones = publicaciones;
            return View("Busqueda", model);
            
            #region Franco
            //var propiedades = _contexto.Propiedades.AsQueryable();  //Recuperar todas las propiedades de base de datos
            //if (model.MinPrecio != null)
            //{
            //    propiedades = propiedades.Where(p => p.PrecioRenta >= model.MinPrecio.Value);
            //}
            //if (model.MaxPrecio != null)
            //{
            //    propiedades = propiedades.Where(p => p.PrecioRenta >= model.MaxPrecio.Value);
            //}
            //if (model.TipoInmueble != null) 
            //{
            //    propiedades = propiedades.Where(p => p.TipoPropiedad == model.TipoInmueble);
            //}
            //if (model.DistanciaAEscuela != null) 
            //{
            //    propiedades = propiedades.Where(p => p.Distancia <= model.DistanciaAEscuela);
            //}
            //var propiedadesList = await propiedades.ToListAsync();  
            //foreach (var currPropiedad in propiedades) 
            //{
            //    var primerImagenPropiedad = await _contexto.Imagenes.FirstOrDefaultAsync(f => f.IdPropiedad == currPropiedad.IdPropiedad);
            //    var mimeType = MimeTypeHelper.GetMimeType(primerImagenPropiedad.Imagen);
            //    model.ListaDePropiedades.Add((primerImagenPropiedad.Imagen, currPropiedad, mimeType));
            //}
            //return View(model);
            #endregion
        }
        #endregion

        #region Detalles
        [HttpGet]
        public async Task<IActionResult> Detalles(int id) 
        {
            //Buscar la propiedad con las sus imagenes
            Propiedade detalles = await _contexto.Propiedades.Include(i => i.Imagenes).FirstAsync(d=>d.IdPropiedad == id);
            return View(detalles);
        }
        #endregion
    }
}
