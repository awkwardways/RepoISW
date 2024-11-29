using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Duda
{
    public int IdDuda { get; set; }

    public int IdPropiedad { get; set; }

    public string Duda1 { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();
}
