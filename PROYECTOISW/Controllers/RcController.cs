using Microsoft.AspNetCore.Mvc;
using PROYECTOISW.Servicios;
using PROYECTOISW.Models.ViewModels.RCViewModels;
using PROYECTOISW.Models.ViewModel;

namespace PROYECTOISW.Controllers
{
    public class RcController : Controller
    {
        private readonly IServicioRC _servicioRC;
        public RcController(IServicioRC servicioRC)
        {
            _servicioRC = servicioRC;
        }

        #region Validar Correo
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
                var model = new TokenContraseña();
                model.ValidarTokenViewModel = new ValidarTokenViewModel();
                model.CambiarContrase =  new CambiarContraseñaViewModel();
                model.ValidarTokenViewModel.Correo = recuperar.Correo;
                model.CambiarContrase.Correo = recuperar.Correo;
                return View("ValidarToken", model);
            }
            return View();
        }
        #endregion

        #region Validar Token
        [HttpGet]
        public IActionResult ValidarToken()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ValidarToken(TokenContraseña model)
        {
            if (model.ValidarTokenViewModel != null)
            {
                // Busca el token con un correo y tokens válidos
                if (await _servicioRC.ValidarCon(model.ValidarTokenViewModel.Correo, Cifrado.GetSHA256(model.ValidarTokenViewModel.Token)) == false)
                {
                    ViewBag.Invalido = "Código no válido.";
                    ViewBag.TokenValido = false;
                    return View(model);
                }
                ViewBag.TokenValido = true;
                return View(model);
                //return View("RestablecerContraseña", new CambiarContraseñaViewModel { Correo = validar.Correo, Token = validar.Token });
            }
            ViewBag.Invalido = "";
            ViewBag.TokenValido = false;
            return View();
        }
        #endregion

        #region Restablecer Contraseña
        [HttpGet]
        public IActionResult RestablecerContraseña()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RestablecerContraseña(TokenContraseña model)
        {
            if (model.CambiarContrase != null)
            {
                //Restablecer contraseña
                await _servicioRC.ActualizarCon(Cifrado.GetSHA256(model.CambiarContrase.Nueva), model.CambiarContrase.Correo);
                ViewBag.Contrase = "Contraseña restablecida con exito";
                return RedirectToAction("IniciarSesion","Usuario");
            }
            return View(model);
        }
        #endregion

        public string GenerarToken()
        {
            Random random = new Random();
            int token = random.Next(1000, 9000);
            return token.ToString();
        }
    }
}
