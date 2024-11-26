using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Rentada
{
    public int IdRentada { get; set; }

    public int IdUsuario { get; set; }

    public int IdPropiedad { get; set; }

    public DateOnly Fecha { get; set; }

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
