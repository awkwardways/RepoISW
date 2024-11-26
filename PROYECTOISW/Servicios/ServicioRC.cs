using Microsoft.EntityFrameworkCore;
using PROYECTOISW.Models;
//Agregar 
using System.Net;
using System.Net.Mail;

namespace PROYECTOISW.Servicios
{
    public class ServicioRC : IServicioRC
    {
        private readonly ProyectoiswContext _contexto;
        public ServicioRC(ProyectoiswContext contexto)
        {
            _contexto = contexto;
        }

        public string BuscarCorreo(string correo)
        {
            var encontrado = (from e in _contexto.Usuarios
                              where e.CorreoElectronico == correo
                              select e.CorreoElectronico).SingleOrDefault();
            return encontrado;
        }

        public async Task GuardarToken(string token, string correo)
        {
            await _contexto.Usuarios
                .Where(c => c.CorreoElectronico == correo)
                .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.Token, token));
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> ValidarCon(string correo, string token)
        {
            var encontrado = await (from e in _contexto.Usuarios
                                    where e.CorreoElectronico == correo && e.Token == token
                                    select e.Token).SingleOrDefaultAsync();
            if (encontrado != null) return true; else return false;
        }

        public async Task ActualizarCon(string nuevaContraseña, string correo)
        {
            try
            {
                await _contexto.Usuarios
                    .Where(c => c.CorreoElectronico == correo)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.Contraseña, nuevaContraseña));
                await _contexto.SaveChangesAsync();
                //Eliminar Token
                await _contexto.Usuarios
                    .Where(c => c.CorreoElectronico == correo)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.Token, string.Empty));
                await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void EnviarCorreo(string destino, string token)
        {
            try
            {
                MailMessage msg = new MailMessage("hernandez.granados.johan.ipn@gmail.com", destino);
                msg.IsBodyHtml = true;
                msg.Subject = "Recuperación de Contraseña";
                string body = $"Debido a su peticion de restablecimiento de contraseña le hemos enviado este correo.<br>Su código de ruperacion es: <br><b>{token}</b>";
                msg.Body = body;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("hernandez.granados.johan.ipn@gmail.com", "kzvl kqnd krhb uwyn");
                smtp.Send(msg);
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
