using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Respuesta
{
    public int IdRespuesta { get; set; }

    public int IdDuda { get; set; }

    public string Respuesta1 { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual Duda IdDudaNavigation { get; set; } = null!;
}
