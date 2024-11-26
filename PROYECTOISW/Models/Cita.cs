using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Cita
{
    public int IdCitas { get; set; }

    public int IdPropiedad { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
