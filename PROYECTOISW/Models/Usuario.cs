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

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<Propiedade> Propiedades { get; set; } = new List<Propiedade>();

    public virtual ICollection<Rentada> Rentada { get; set; } = new List<Rentada>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
}
