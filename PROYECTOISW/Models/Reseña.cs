using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Reseña
{
    public int IdReseña { get; set; }

    public int IdUsuario { get; set; }

    public int IdPropiedad { get; set; }

    public string Comentario { get; set; } = null!;

    public int? Calificacion { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public virtual Propiedade IdPropiedadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
