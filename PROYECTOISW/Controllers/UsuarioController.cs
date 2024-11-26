using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Models;
using PROYECTOISW.Servicios;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PROYECTOISW.Models.ViewModel.PerfilViewModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PROYECTOISW.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ProyectoiswContext _contexto;
     
        public UsuarioController(ProyectoiswContext context) 
        {
            _contexto = context;
        }
        #region Crear
        [HttpGet]
        public IActionResult CrearUsuario()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CrearUsuario(CrearUsuarioViewModel nuevo, IFormFile? Foto)
        {
            if (ModelState.IsValid)
            {
                //Verificar el tipo de usuario
                nuevo.Tipo = nuevo.Tipo == "estudiante" ? "A" : "P";

                //Verifiacar la foto
                if (Foto != null && Foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Foto.CopyToAsync(memoryStream);
                        nuevo.Foto = memoryStream.ToArray();
                    }
                }
                else
                {
                    //Si la foto viene vacia le asginamos una foto en el proyecto
                    string rutaArchivo = Path.Combine("wwwroot", "Imagenes", "perfil.png");
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        nuevo.Foto = memoryStream.ToArray();
                    }
                }

                //Validar el correo en caso de ser alumno
                if (nuevo.Tipo == "A")
                {
                    string emailPattern = @"^[^@\s]+@alumno\.ipn\.mx$";
                    if (!Regex.IsMatch(nuevo.CorreoElectronico, emailPattern, RegexOptions.IgnoreCase))
                    {
                        ViewBag.EmailError = "El correo electrónico debe pertenecer al dominio alumno.ipn.mx si eres estudiante.";
                        return View(nuevo); // Regresar a la vista con el modelo para mostrar el error
                    }
                }

                //Guardar usuario
                var crear = new Usuario
                {
                    Tipo = nuevo.Tipo,
                    NombreCompleto = nuevo.NombreCompleto,
                    CorreoElectronico = nuevo.CorreoElectronico,
                    Contraseña =Cifrado.GetSHA256(nuevo.Contraseña),
                    Telefono = nuevo.Telefono,
                    Foto = nuevo.Foto
                };
                _contexto.Usuarios.Add(crear);
                await _contexto.SaveChangesAsync();

                //Agrega el usuario a cookie
                var claims = new List<Claim>
                {
                    new Claim("Id_Usuario", crear.IdUsuario.ToString()),
                    new Claim("Tipo", crear.Tipo),
                    new Claim(ClaimTypes.Name, crear.NombreCompleto),
                    new Claim(ClaimTypes.Email, crear.CorreoElectronico),
                    new Claim(ClaimTypes.MobilePhone, crear.Telefono),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home"); // Redirigir después de guardar
            }
            return View(nuevo);
        }
        #endregion

        #region Inicar sesion
        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(IniciarSesionViewModel entrar)
        {
            if (ModelState.IsValid)
            {
                var usuario = await(from u in _contexto.Usuarios
                                    where u.CorreoElectronico == entrar.Correo && u.Contraseña == Cifrado.GetSHA256(entrar.Contraseña)
                                    select u).FirstOrDefaultAsync();
                if (usuario == null)
                {
                    ViewBag.Invalido = "Usuario no encontrado";
                    return View(entrar);
                }

                //Agrega el usuario a cookie
                var claims = new List<Claim>
                {
                    new Claim("Id_Usuario", usuario.IdUsuario.ToString()),
                    new Claim("Tipo", usuario.Tipo),
                    new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                    new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                    new Claim(ClaimTypes.MobilePhone, usuario.Telefono)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home", new { id = usuario.IdUsuario, opcion = 1});
            }
            return View(entrar);
        }
        #endregion

        #region Cerrar Sesion
        [HttpGet]
        public IActionResult CerrarSesion()
        {
            // Clear the existing external cookie
            BorrarCookie();
            return RedirectToAction("IniciarSesion", "Usuario");
        }
        #endregion

        #region Cambiar Contraseña
        [HttpGet]
        public IActionResult CambiarContraseña()
        {
            //Deserealizar la cooki y obtener el id y el correo
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var id = int.Parse(claimsIdentity.FindFirst("Id_Usuario")?.Value);
                var correo = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value.ToString();
                CambiarContraseñaViewModel nueva = new CambiarContraseñaViewModel();
                nueva.Id = id;
                nueva.Correo = correo;
                return View(nueva);
            }
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CambiarContraseña(CambiarContraseñaViewModel nueva)
        {
            if (ModelState.IsValid)
            {
                //1. Buscar la contraseña del usuario, ya que no se almacena en cookie
                var encontrar = await (from e in _contexto.Usuarios
                                       where e.IdUsuario == nueva.Id &&
                                       e.CorreoElectronico == nueva.Correo &&
                                       e.Contraseña == Cifrado.GetSHA256(nueva.Actual)
                                       select e).FirstOrDefaultAsync();

                //Si la contraseña es incorrecta
                if (encontrar == null)
                {
                    ViewBag.Error = "Contraseña actual incorrecta.";
                    return View(nueva);
                }

                //2. Actualizar la contraseña
                await _contexto.Usuarios
                   .Where(c => c.CorreoElectronico == nueva.Correo && c.IdUsuario == nueva.Id)
                   .ExecuteUpdateAsync(setters => 
                   setters.SetProperty(t => t.Contraseña, Cifrado.GetSHA256(nueva.ConfirmarContraseña)));
                await _contexto.SaveChangesAsync();

                //Cerrar la sesion actual
                BorrarCookie();
                return RedirectToAction("IniciarSesion", "Usuario");
            }
            return View();
        }
        #endregion

        #region Cambiar Correo
        [HttpGet]
        public IActionResult CambiarCorreo()
        {
            //1. Obtener el Id del usuario guardado en la cookie
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var id = int.Parse(claimsIdentity.FindFirst("Id_Usuario")?.Value);
                CambiarCorreoViewModel nueva = new CambiarCorreoViewModel();
                nueva.Id = id;
                return View(nueva);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarCorreo(CambiarCorreoViewModel nuevo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //1. Buscar la contraseña y el id
                    var encontrado = await (from e in _contexto.Usuarios
                                            where e.IdUsuario == nuevo.Id && e.Contraseña == Cifrado.GetSHA256(nuevo.Contraseña)
                                            select e).FirstOrDefaultAsync();
                    if (encontrado == null)
                    {
                        ViewBag.Error = "Contraseña Incorrecta.";
                        return View(nuevo);
                    }

                    //2. Actualizar correo
                    await _contexto.Usuarios
                    .Where(c => c.IdUsuario == nuevo.Id && c.Contraseña ==Cifrado.GetSHA256(nuevo.Contraseña))
                    .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(t => t.CorreoElectronico, nuevo.NuevoCorreo));
                    await _contexto.SaveChangesAsync();

                    //Cerrar la sesion actual
                    BorrarCookie();
                    return RedirectToAction("IniciarSesion", "Usuario");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View();
        }
        #endregion

        #region Cambiar Telefono
        [HttpGet]
        public IActionResult CambiarTelefono()
        {
            //1. Obtener el Id del usuario guardado en la cookie
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var id = int.Parse(claimsIdentity.FindFirst("Id_Usuario")?.Value);
                CambiarTelefonoViewModel nueva = new CambiarTelefonoViewModel();
                nueva.Id = id;
                return View(nueva);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CambiarTelefono(CambiarTelefonoViewModel nuevo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //1. Buscar usuario por id
                    var encontrado =await (from e in _contexto.Usuarios
                                    where e.IdUsuario == nuevo.Id && e.Contraseña == Cifrado.GetSHA256(nuevo.Contraseña)
                                    select e).FirstOrDefaultAsync();
                    if (encontrado == null)
                    {
                        ViewBag.Error = "Contraseña Incorrecta.";
                        return View(nuevo);
                    }

                    //2. Actualiza el telefono
                    await _contexto.Usuarios
                    .Where(c => c.IdUsuario == nuevo.Id && c.Contraseña == Cifrado.GetSHA256(nuevo.Contraseña))
                    .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(t => t.Telefono, nuevo.NuevoTelefono));
                    await _contexto.SaveChangesAsync();
                    return View();
                    //No es necesario cerrar la sesión
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);  
                }
            }
            return View(nuevo);
        }
        #endregion

        #region Cambiar Imagen
        [HttpGet]
        public IActionResult CambiarImagen()
        {
            //1. Obtener el Id del usuario guardado en la cookie
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var id = int.Parse(claimsIdentity.FindFirst("Id_Usuario")?.Value); 
                CambiarImagenViewModel nueva = new CambiarImagenViewModel();
                nueva.Id = id;
                return View(nueva);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CambiarImagen(CambiarImagenViewModel nueva, IFormFile? Imagen)
        {
            //Asigna la imagen
            //Asigna la imagen
            if (Imagen != null && Imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Imagen.CopyToAsync(memoryStream);
                    nueva.Imagen = memoryStream.ToArray();
                }
            }
            //Guardar en BD
            await _contexto.Usuarios
                   .Where(c => c.IdUsuario == nueva.Id)
                   .ExecuteUpdateAsync(setters =>
                   setters.SetProperty(t => t.Foto, nueva.Imagen));
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { id = nueva.Id, opcion = 1 });
        }
        #endregion 
        public async void BorrarCookie()
        {
            await HttpContext.SignOutAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
