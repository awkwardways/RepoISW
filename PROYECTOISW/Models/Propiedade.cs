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

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Duda> Duda { get; set; } = new List<Duda>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Imagene> Imagenes { get; set; } = new List<Imagene>();

    public virtual ICollection<Rentada> Rentada { get; set; } = new List<Rentada>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
}
