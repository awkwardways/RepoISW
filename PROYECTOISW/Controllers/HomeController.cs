using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Models;
using PROYECTOISW.Models.ViewModel.PerfilViewModel;
using System.Diagnostics;

//Using agregado
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PROYECTOISW.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProyectoiswContext _contexto;

        public HomeController(ILogger<HomeController> logger, ProyectoiswContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }
        [HttpGet]
        public async Task <IActionResult> Index(int id, int opcion)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var usuario = new UsuarioViewModel();
            var u = await _contexto.Usuarios.Where(i => i.IdUsuario == id).FirstOrDefaultAsync();
            //Inicio de sesion y consulta de cookies
            var tipo = claimsIdentity?.FindFirst("Tipo")?.Value;
            var nombre = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
            usuario.Tipo = tipo;
            usuario.NombreCompleto = nombre;
            usuario.Foto = u.Foto;
            if (opcion == 1)
            {
                var n = await _contexto.Usuarios.Where(i => i.IdUsuario == id).FirstOrDefaultAsync();
                if (n != null)
                {
                    usuario.Foto = n.Foto;
                }
            }
            return View(usuario);
        }
        [HttpGet]
        public JsonResult CheckAlquiladas()
        {
            bool tienePropiedadesAlquiladas = Alquiladas();
            return Json(new { tienePropiedadesAlquiladas });
        }
        public bool Alquiladas()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            var rentadas = _contexto.Rentadas.Where(id => id.IdUsuario == int.Parse(idUser)).FirstOrDefault();
            if (rentadas != null)
                return true;
            else return false;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
