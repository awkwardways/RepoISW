using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Servicios;
using PROYECTOISW.Models.ViewModels.RCViewModels;

namespace PROYECTOISW.Controllers
{
    public class RcController : Controller
    {
        private readonly IServicioRC _servicioRC;
        public RcController(IServicioRC servicioRC)
        {
            _servicioRC = servicioRC;
        }
        [HttpGet]
        public IActionResult ValidarCorreo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidarCorreo(ValidarCorreoViewModel recuperar)
        {
            //Cifra el token
            string token = GenerarToken();

            if (ModelState.IsValid)
            {
                var encontrado = _servicioRC.BuscarCorreo(recuperar.Correo);

                if (encontrado == null)
                {
                    ViewBag.Invalido = "El correo no esta registrado en el sistema";
                    return View(recuperar);
                }
                //Genera toquen
                _servicioRC.GuardarToken(Cifrado.GetSHA256(token), encontrado);
                //Mandar alerta de nuevo token
                _servicioRC.EnviarCorreo(encontrado, token);
                return View("ValidarToken", new ValidarTokenViewModel { Correo = recuperar.Correo });
            }
            return View();
        }

        [HttpGet]
        public IActionResult ValidarToken()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ValidarToken(ValidarTokenViewModel validar)
        {
            if (ModelState.IsValid)
            {
                //Busca el token con un correo y tokens validos
                if (await _servicioRC.ValidarCon(validar.Correo, Cifrado.GetSHA256(validar.Token)) == false)
                {
                    ViewBag.Invalido = "Codigo no valido.";
                    return View(validar);
                }
                ViewBag.TokenValido = true;
                return View("RestablecerContraseña", new CambiarContraseñaViewModel { Correo = validar.Correo, Token = validar.Token });
            }
            return View();
        }
        [HttpGet]
        public IActionResult RestablecerContraseña()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RestablecerContraseña(CambiarContraseñaViewModel cambiar)
        {
            if (ModelState.IsValid)
            {
                //Restablecer contraseña
                await _servicioRC.ActualizarCon(Cifrado.GetSHA256(cambiar.Nueva), cambiar.Correo);
                ViewBag.Contrase = "Contraseña restablecida con exito";
                return RedirectToAction("IniciarSesion","Usuario");
            }
            return View();
        }
        public string GenerarToken()
        {
            Random random = new Random();
            int token = random.Next(1000, 9000);
            return token.ToString();
        }
    }
}
