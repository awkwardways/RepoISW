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
        #region Rentar
        [HttpGet]
        public async Task <IActionResult> Rentar(int idPropiedad)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            Rentada rentar = new Rentada();
            rentar.IdPropiedad = idPropiedad;
            rentar.IdUsuario = Convert.ToInt32(idUser);
            await _contexto.Rentadas.AddAsync(rentar);
            await _contexto.SaveChangesAsync();
            //Actualiza el estado de la propiedad en la tabla de propiedades
            await _contexto.Propiedades
                    .Where(id => id.IdPropiedad == idPropiedad)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, "D"));
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Rentadas");
        }
        #endregion

        #region Alquiladas
        [HttpGet]
        public async Task <IActionResult> Rentadas()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            var rentadas = await _contexto.Rentadas
                .Where(f => f.IdUsuario == int.Parse(idUser))
                .Include(f => f.IdPropiedadNavigation) // Asegúrate de que 'Propiedad' es la navegación correcta
                .ThenInclude(p => p.Imagenes) // Asegúrate de que 'Imagenes' es la navegación correcta
                .Select(f => f.IdPropiedadNavigation)
                .ToListAsync();
            return View(rentadas);
        }
        #endregion
    }
}
