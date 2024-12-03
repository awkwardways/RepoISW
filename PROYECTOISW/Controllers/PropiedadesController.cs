using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PROYECTOISW.Models;
using System.IO;

//Agregar using
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using PROYECTOISW.Models.ViewModel.PropiedadViewModel;
using PROYECTOISW.Models.ViewModel.PerfilViewModel;
using System.Reflection.Metadata.Ecma335;
using NuGet.Protocol.Plugins;
using PROYECTOISW.Servicios;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace PROYECTOISW.Controllers
{
    [Authorize]
    public class PropiedadesController : Controller
    {
        private readonly ProyectoiswContext _contexto;
        private readonly IServicioRC _servicios;

        public PropiedadesController(ProyectoiswContext contexto, IWebHostEnvironment hostingEnvironment, IServicioRC servicios)
        {
            _contexto = contexto;
            _servicios = servicios;
        }

        #region Crear
        [Authorize(Roles = "P")]
        [HttpGet]
        public IActionResult CrearPropiedad()
        {
            var model = new CrearPropiedadViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPropiedad(CrearPropiedadViewModel nuevo)
        {
            if (ModelState.IsValid)
            {
                //Deseralizar una cookie
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var id = int.Parse(claimsIdentity.FindFirst("Id_Usuario")?.Value);
                    // System.Diagnostics.Debug.WriteLine("Se subieron fotos\n");
                    var crear = new Propiedade
                    {
                        Estado = "H",
                        IdUsuario = id,
                        Titulo = nuevo.Titulo,
                        Descripcion = nuevo.Descripcion,
                        TipoPropiedad = nuevo.TipoPropiedad,
                        PrecioRenta = nuevo.PrecioRenta,
                        Superficie = nuevo.Superficie,
                        NumeroHabitaciones = nuevo.NumeroHabitaciones,
                        NumeroBaños = nuevo.NumeroBaños,
                        Servicios = nuevo.Servicios,
                        Direccion = nuevo.Direccion,
                        Distancia = nuevo.Distancia,
                        CondicionesEspeciales = nuevo.CondicionesEspeciales,
                        FechaPublicacion = DateTime.Now,
                    };
                    _contexto.Propiedades.Add(crear);
                    await _contexto.SaveChangesAsync();
                    //Valida que no sean más de 10 imagenes
                    if (nuevo.archivosImagenes.Count > 10)
                    {
                        ViewBag.Imagenes = "No se permiten más de 10 imagenes";
                        return View(nuevo);
                    }

                    if (nuevo.archivosImagenes != null && nuevo.archivosImagenes.Count > 0)
                    {
                        foreach (var foto in nuevo.archivosImagenes)
                        {
                            if (foto.Length > 0)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    await foto.CopyToAsync(memoryStream);
                                    var imagen = new Imagene
                                    {
                                        IdPropiedad = crear.IdPropiedad,
                                        Imagen = memoryStream.ToArray()
                                    };
                                    _contexto.Imagenes.Add(imagen);
                                }
                            }
                        }
                        await _contexto.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Home", new { id = id, opcion = 0 });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }
            return View(nuevo);
        }
        #endregion

        #region MostrarPrpopiedades
        [HttpGet]
        public async Task<IActionResult> GestionarPropiedades()
        {
            // Esta vista contendrá todas las publicaciones realizadas por el propietario
            // en donde cada propiedad tendrá botones para que el usuario pueda editar, eliminar o poner 
            // en suspensión una publicación
            // 1. Hay que consultar el id del propietario en la cookie
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                try
                {
                    var id = Convert.ToInt32(claimsIdentity.FindFirst("Id_Usuario")?.Value);
                    var propiedades = await _contexto.Propiedades
                        .Where(e => e.IdUsuario == id)
                        .Include(p => p.Imagenes) // Incluye las imágenes relacionadas
                        .Include(p => p.Duda) // Incluye las reseñas relacionadas
                        .ToListAsync();

                    return View(propiedades);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View();
        }
        #endregion

        #region Editar Propiedad
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var propiedades = await _contexto.Propiedades
                       .Where(e => e.IdPropiedad == id)
                       .Include(p => p.Imagenes) // Incluye las imágenes relacionadas
                       .FirstOrDefaultAsync();
                var editar = new EditarPropiedadViewModel();
                editar.Titulo = propiedades.Titulo;
                editar.Descripcion = propiedades.Descripcion;
                editar.TipoPropiedad = propiedades.TipoPropiedad;
                editar.PrecioRenta = propiedades.PrecioRenta;
                editar.Superficie = propiedades.Superficie;
                editar.NumeroHabitaciones = propiedades.NumeroHabitaciones;
                editar.NumeroBaños = propiedades.NumeroBaños;
                editar.Servicios = propiedades.Servicios;
                editar.Direccion = propiedades.Direccion;
                editar.Distancia = propiedades.Distancia;
                editar.CondicionesEspeciales = propiedades.CondicionesEspeciales;
                editar.Data = new List<byte[]>();

                foreach (var item in propiedades.Imagenes)
                {
                    editar.Data.Add(item.Imagen);
                }

                return View(editar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Editar(EditarPropiedadViewModel editar)
        {
            if (ModelState.IsValid)
            {
                var imagenes = await _contexto.Imagenes.Where(i => i.IdPropiedad == editar.Id).ToListAsync();
              

                await _contexto.Propiedades
                       .Where(d => d.IdPropiedad == editar.Id)
                       .ExecuteUpdateAsync(setters =>
                       setters.SetProperty(t =>
                       t.Titulo, editar.Titulo)
                       .SetProperty(t => t.Descripcion, editar.Descripcion)
                       .SetProperty(t => t.TipoPropiedad, editar.TipoPropiedad)
                       .SetProperty(t => t.PrecioRenta, editar.PrecioRenta)
                       .SetProperty(t => t.Superficie, editar.Superficie)
                       .SetProperty(t => t.NumeroHabitaciones, editar.NumeroHabitaciones)
                       .SetProperty(t => t.NumeroBaños, editar.NumeroBaños)
                       .SetProperty(t => t.Servicios, editar.Servicios)
                       .SetProperty(t => t.Direccion, editar.Direccion)
                       .SetProperty(t => t.Distancia, editar.Distancia)
                       .SetProperty(t => t.CondicionesEspeciales, editar.CondicionesEspeciales
                       ));
                await _contexto.SaveChangesAsync();

                //agregar las fotos
                if (editar.archivosImagenes != null && editar.archivosImagenes.Count > 0)
                {
                    if (imagenes.Count == 10)
                    {
                        ViewBag.Imagenes = "El número maximo de imagenes por proiedad es 10.";
                        return View(editar);
                    }
                    foreach (var foto in editar.archivosImagenes)
                    {
                        if (foto.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await foto.CopyToAsync(memoryStream);
                                var imagen = new Imagene
                                {
                                    IdPropiedad = editar.Id,
                                    Imagen = memoryStream.ToArray()
                                };
                                _contexto.Imagenes.Add(imagen);
                            }
                        }
                    }
                    await _contexto.SaveChangesAsync();
                }
                return RedirectToAction(nameof(GestionarPropiedades));
            }
            return View(editar);
        }
        #endregion

        #region Eliminar Propiedad
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            //1. Eliminar las fotos de en la tabla imagenes
            var e = await _contexto.Imagenes.Where(w => w.IdPropiedad == id).ToListAsync();
            var d = await _contexto.Dudas.Where(w => w.IdPropiedad == id).ToListAsync();
            var f = await _contexto.Favoritos.Where(f => f.IdPropiedad == id).ToListAsync();
            var c = await _contexto.Reseñas.Where(c => c.IdPropiedad == id).ToListAsync();
            var r = await _contexto.Rentadas.Where(r => r.IdPropiedad == id).ToListAsync();
            if (e.Any())
            {
                foreach (var el in e)
                {
                    _contexto.Imagenes.Remove(el);
                }
                await _contexto.SaveChangesAsync();
            }
            if (d.Any())
            {
                foreach (var du in d)
                {
                    _contexto.Dudas.Remove(du);
                }
                await _contexto.SaveChangesAsync();
            }
            if (f.Any())
            {
                foreach (var fa in f)
                {
                    _contexto.Favoritos.Remove(fa);
                }
                await _contexto.SaveChangesAsync();
            }
            if (c.Any())
            {
                foreach (var re in c)
                {
                    _contexto.Reseñas.Remove(re);
                }
                await _contexto.SaveChangesAsync();
            }
            if (r.Any())
            {
                foreach (var rentada in r)
                {
                    _contexto.Rentadas.Remove(rentada);
                }
                await _contexto.SaveChangesAsync();
            }
            //Eliminar la propiedad
            Propiedade elimiar = await _contexto.Propiedades.FirstAsync(e => e.IdPropiedad == id);
            _contexto.Propiedades.Remove(elimiar);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(GestionarPropiedades));
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Pausar(int id, int op)
        {
            string estado = "";
            if (op == 1) 
                estado = "D"; 
            else 
            {
                var rentada = await _contexto.Rentadas.Where(i => i.IdPropiedad == id).FirstAsync();
                if (rentada != null) 
                    _contexto.Rentadas.Remove(rentada);
                estado = "H"; 
            }
            await _contexto.Propiedades.Where(i => i.IdPropiedad == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.Estado, estado));
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(GestionarPropiedades));
        }
        #region Contactar
        [HttpGet]
        public async Task<IActionResult> Contactar(int idUser, int idPropiedad)
        {
            //Buscamos el usuario en la tabla usuarios para recuperar su correo
            var emailProp = await _contexto.Usuarios.Where(p => p.IdUsuario == idUser).Select(c => c.CorreoElectronico).FirstOrDefaultAsync();
            if (emailProp != null)
            {
                //Obtener el correo del usuario
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var emailUser = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
                var mobilPhone = claimsIdentity?.FindFirst(ClaimTypes.MobilePhone)?.Value;
                var nameUser = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
                Usuario user = new Usuario();
                user.Telefono = mobilPhone;
                user.NombreCompleto = nameUser;
                user.CorreoElectronico = emailUser;
                //Manda el correo
                _servicios.EnviarCorreo(emailProp, user, idPropiedad);
            }
            return Content("");
        }
        #endregion
    }
}
