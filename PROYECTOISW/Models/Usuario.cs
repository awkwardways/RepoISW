using System;
using System.Collections.Generic;

namespace PROYECTOISW.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Tipo { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string? Token { get; set; }

    public string Telefono { get; set; } = null!;

    public byte[] Foto { get; set; } = null!;  

    public virtual ICollection<Propiedade> Propiedades { get; set; } = new List<Propiedade>();
}
