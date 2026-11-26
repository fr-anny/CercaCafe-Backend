using System;
using System.Collections.Generic;

namespace CercaCafe.DAL.Models;
public partial class Cafeteria
{
    public int IdCafeteria { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Telefono { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public decimal? Calificacion { get; set; }

    public string? TipoCafeteria { get; set; }
    public string? ImagenUrl { get; set; }
    public string? Descripcion { get; set; }
    public bool Wifi { get; set; } = true;

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
