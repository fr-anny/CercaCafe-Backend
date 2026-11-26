using System;
using System.Collections.Generic;

namespace CercaCafe.DAL.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateOnly FechaRegistro { get; set; }

    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
    public string Rol { get; set; } = "Cliente";
}