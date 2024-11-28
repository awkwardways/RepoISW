using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Models;
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
        public async Task <IActionResult> Comentar(ComentarioViewModel nueva)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idUser = claimsIdentity?.FindFirst("Id_Usuario")?.Value;
            Reseña n = new Reseña();
            n.Calificacion = nueva.Calificacion;
            n.Comentario = nueva.Comentario;
            n.IdUsuario =Convert.ToInt32(idUser);
            n.IdPropiedad = nueva.IdPropiedad;
            n.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            await _contexto.Reseñas.AddAsync(n);
            await _contexto.SaveChangesAsync();
            return Content("");
        }
        [HttpGet]
        public async Task <IActionResult> Mostrar()
        {
            var comentarios = await _contexto.Reseñas.Where(c => c.IdPropiedad == )
            return View();
        }
    }
}
