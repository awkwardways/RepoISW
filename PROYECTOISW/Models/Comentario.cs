using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Comentario
{
    public int IdComentario { get; set; }

    public int IdUsuario { get; set; }

    public int IdPropiedad { get; set; }

    public string Comentario1 { get; set; } = null!;

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
