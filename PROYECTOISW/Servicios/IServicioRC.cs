using PROYECTOISW.Models;

namespace PROYECTOISW.Servicios
{
    public interface IServicioRC
    {
        string BuscarCorreo(string correo);
        Task<bool> ValidarCon(string correo, string token);
        Task GuardarToken(string token, string correo);
        Task ActualizarCon(string nuevaCon, string correo);

        void EnviarCorreo(string destino, string token);
    }
}
