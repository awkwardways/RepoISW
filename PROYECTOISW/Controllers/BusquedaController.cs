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
using PROYECTOISW.Servicios;
using PROYECTOISW.Models.ViewModel.ComentariosViewModel;
using PROYECTOISW.Models.ViewModel.ContactarViewModel;

namespace PROYECTOISW.Controllers
{
    [Authorize]
    public class BusquedaController : Controller
    {
        private readonly ProyectoiswContext _contexto;
        private readonly IServicioRC _servicioC;
        public BusquedaController(ProyectoiswContext context, IServicioRC servicioC)
        {
            _contexto = context;
            _servicioC = servicioC;
        }

        #region Busqueda
        [HttpGet]
        public async Task<IActionResult> Busqueda()
        {
            var publicaciones = await _contexto.Propiedades.Where(e => e.Estado != "D").Include(p => p.Imagenes).ToListAsync();
            var viewModel = new CompartidoViewModel();
            viewModel.Publicaciones = new List<Propiedade>();
            viewModel.Buscar = new BusquedaViewModel();

            viewModel.Publicaciones = publicaciones;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Busqueda(CompartidoViewModel model)
        {
            //Agregar filtros
            var publicaciones = await _contexto.Propiedades
                    .Where(p => p.TipoPropiedad == model.Buscar.TipoInmueble &&
                    p.Distancia >= model.Buscar.DistanciaAEscuela &&
                    (p.PrecioRenta <= model.Buscar.MaxPrecio) &&
                    p.Estado != "D")
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
            Propiedade detalles = await _contexto.Propiedades.Include(i => i.Imagenes).FirstAsync(d => d.IdPropiedad == id);
            return View(detalles);
        }
        #endregion

        #region Rentar
        [HttpGet]
        public async Task<IActionResult> Rentar(int idUser, int idPropiedad)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUsuario = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            Rentada rentar = new Rentada();
            rentar.IdPropiedad = idPropiedad;
            rentar.IdUsuario = Convert.ToInt32(idUsuario);
            await _contexto.Rentadas.AddAsync(rentar);
            await _contexto.SaveChangesAsync();
            //Actualiza el estado de la propiedad en la tabla de propiedades
            await _contexto.Propiedades
                    .Where(id => id.IdPropiedad == idPropiedad)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, "D"));
            await _contexto.SaveChangesAsync();
            var emailProp = await _contexto.Usuarios.Where(p => p.IdUsuario == idUser).Select(c => c.CorreoElectronico).FirstOrDefaultAsync();
            if (emailProp != null)
            {
                //Obtener el correo del usuario
                var emailUser = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
                var mobilPhone = claimsIdentity?.FindFirst(ClaimTypes.MobilePhone)?.Value;
                var nameUser = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
                Usuario user = new Usuario();
                user.Telefono = mobilPhone;
                user.NombreCompleto = nameUser;
                user.CorreoElectronico = emailUser;
                //Manda el correo
                _servicioC.EnviarCorreo(emailProp, user, idPropiedad);
            }
            return RedirectToAction("Rentadas");
        }
        #endregion

        #region Alquiladas
        public async Task<IActionResult> Rentadas()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            ComentarioPropiedadViewModel model = new ComentarioPropiedadViewModel
            {
                Comentario = new ComentarioViewModel(),
                Propiedad = new List<Propiedade>(),
                reseñas = new List<bool>()
            };

            var r = await _contexto.Rentadas
                .Where(f => f.IdUsuario == int.Parse(idUser))
                .Include(f => f.IdPropiedadNavigation)
                .ThenInclude(p => p.Imagenes)
                .Select(f => f.IdPropiedadNavigation)
                .ToListAsync();


            model.Propiedad = r;

            Reseña? encontrada =  new Reseña();
            foreach(var reseña in r)
            {
                encontrada = await _contexto.Reseñas.Where(id => id.IdPropiedad == reseña.IdPropiedad && id.IdUsuario == Convert.ToInt16(idUser)).FirstOrDefaultAsync();
                if (encontrada != null)
                    model.reseñas.Add(true);
                else
                    model.reseñas.Add(false);
            }
            return View(model);
        }
        #endregion

        #region Contactar
        [HttpGet]
        // Método Contactar
        public async Task<IActionResult> Contactar(int IdPropiedad)
        {
            var dudasConRespuestas = await _contexto.Dudas
                .Where(d => d.IdPropiedad == IdPropiedad)
                .Include(d => d.Respuesta)
                .ToListAsync();

            if (dudasConRespuestas.Count == 0)
            {
                ContactarViewModel n = new ContactarViewModel
                {
                    IdPropiedad = IdPropiedad
                };
                return PartialView("~/Views/Busqueda/CrearDuda.cshtml", n);
            }

            return View(dudasConRespuestas);
        }
        [HttpPost]
        // Método CrearDuda
        public async Task<IActionResult> CrearDuda(ContactarViewModel nueva)
        {
            if (ModelState.IsValid)
            {
                Duda n = new Duda();
                n.Duda1 = nueva.Duda;
                n.IdPropiedad = nueva.IdPropiedad;
                n.FechaCreacion = DateTime.Now;
                await _contexto.Dudas.AddAsync(n);
                await _contexto.SaveChangesAsync();
                //Mandar correo de alerta
                var emailProp = await _contexto.Propiedades
                    .Where(p => p.IdPropiedad == n.IdPropiedad)
                    .Include(p => p.IdUsuarioNavigation)
                    .Select(p => p.IdUsuarioNavigation.CorreoElectronico) // Asegúrate de que 'Email' es el nombre correcto de la propiedad
                    .FirstOrDefaultAsync();
                if (emailProp != null)
                {
                    //Manda el correo
                    _servicioC.EnviarCorreo(emailProp);
                }
                return RedirectToAction(nameof(Contactar), new { IdPropiedad = nueva.IdPropiedad });
            }
            return View(nameof(CrearDuda));
        }
        #endregion
    }
}
