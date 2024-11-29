using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROYECTOISW.Models;
using PROYECTOISW.Models.ViewModel;
using PROYECTOISW.Models.ViewModel.ComentariosViewModel;
using System.Security.Claims;

namespace PROYECTOISW.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly ProyectoiswContext _contexto;
        public ComentariosController(ProyectoiswContext contexto) 
        {
            _contexto = contexto;
        }
        [HttpPost]
        public async Task<IActionResult> Comentar(ComentarioPropiedadViewModel comen)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            Reseña n = new Reseña
            {
                Calificacion = comen.Comentario.Calificacion,
                Comentario = comen.Comentario.Comentario,
                IdUsuario = Convert.ToInt32(idUser),
                IdPropiedad = comen.Comentario.IdPropiedad,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Now)
            };
            await _contexto.Reseñas.AddAsync(n);
            await _contexto.SaveChangesAsync();
            return Content("");
        }
        [HttpGet]
        public async Task <IActionResult> Mostrar(int idP)
        {
            var comentarios = await _contexto.Reseñas
                .Where(c => c.IdPropiedad == idP)
                .Include(id=> id.IdUsuarioNavigation)
                .ToListAsync();
            return View(comentarios);
        }
    }
}
