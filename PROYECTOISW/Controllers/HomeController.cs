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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
