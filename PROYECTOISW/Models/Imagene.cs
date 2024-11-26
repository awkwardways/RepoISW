using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Imagene
{
    public int IdFoto { get; set; }

    public int IdPropiedad { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;
}
