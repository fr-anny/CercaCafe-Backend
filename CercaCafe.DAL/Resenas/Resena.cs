using CercaCafe.DAL.Models;
using System;
using System.Collections.Generic;

namespace CercaCafe.DAL.Models;

public partial class Resena
{
    public int IdResena { get; set; }

    public int IdUsuario { get; set; }

    public int IdCafeteria { get; set; }

    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateOnly FechaResena { get; set; }
    public string? FotoUrl { get; set; }

    public virtual Cafeteria IdCafeteriaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
