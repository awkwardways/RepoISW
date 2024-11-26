using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Propiedade
{
    public int IdPropiedad { get; set; }

    public int IdUsuario { get; set; }

    public string Estado { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string TipoPropiedad { get; set; } = null!;

    public int PrecioRenta { get; set; }

    public string Superficie { get; set; } = null!;

    public string NumeroHabitaciones { get; set; } = null!;

    public string NumeroBaños { get; set; } = null!;

    public string Servicios { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Distancia { get; set; }

    public string CondicionesEspeciales { get; set; } = null!;

    public DateTime FechaPublicacion { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Imagene> Imagenes { get; set; } = new List<Imagene>();
}
