using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROYECTOISW.Models;
using PROYECTOISW.Models.ViewModel.PerfilViewModel;
using System.Security.Claims;

namespace PROYECTOISW.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ProyectoiswContext _contexto;
        public FavoritosController(ProyectoiswContext contexto)
        {
            _contexto = contexto;
        }
        #region Mostrar Favoritos
        [HttpGet]
        public async Task<IActionResult> Mostrar()
        {
            //Deserealizar la cookie y obtener id de usuario
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            if (idUser != null)
            {
                var propiedadesFavoritas = await _contexto.Favoritos
     .Where(f => f.IdUsuario == int.Parse(idUser))
     .Include(f => f.IdPropiedadNavigation) // Asegúrate de que 'Propiedad' es la navegación correcta
         .ThenInclude(p => p.Imagenes) // Asegúrate de que 'Imagenes' es la navegación correcta
     .Select(f => f.IdPropiedadNavigation)
     .ToListAsync();
                return View(propiedadesFavoritas);
            }
            return View();

        }
        #endregion

        #region Crear Favoritos
        [HttpPost]
        public async Task<IActionResult> Favoritos(int idPropiedad)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            Favorito f = new Favorito();
            f.IdPropiedad = idPropiedad;
            f.IdUsuario = int.Parse(idUser);
            await _contexto.Favoritos.AddAsync(f);
            await _contexto.SaveChangesAsync();
            return Content("");
        }
        #endregion

        #region Eliminar
        [HttpPost]
        public async Task<IActionResult> Eliminar(int idPropiedad)
        {
            var favorito = await _contexto.Favoritos
                .FirstOrDefaultAsync(f => f.IdPropiedad == idPropiedad);

            if (favorito != null)
            {
                _contexto.Favoritos.Remove(favorito);
                await _contexto.SaveChangesAsync();
                return Json(new { success = true, idPropiedad = idPropiedad });
            }

            return Json(new { success = false });
        }
        #endregion
    }
}
