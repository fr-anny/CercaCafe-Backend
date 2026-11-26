using CercaCafe.DAL.Models;
using System;
using System.Collections.Generic;

namespace CercaCafe.DAL.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCafeteria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Categoria { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public string? Imagen { get; set; }

    public virtual Cafeteria IdCafeteriaNavigation { get; set; } = null!;
}
