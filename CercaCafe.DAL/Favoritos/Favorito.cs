using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CercaCafe.DAL.Models
{
    public class Favorito
    {
        public int IdFavorito { get; set; }
        public int IdUsuario { get; set; }
        public int IdCafeteria { get; set; }
        public DateTime? FechaGuardado { get; set; }
        public virtual Cafeteria IdCafeteriaNavigation { get; set; } = null!;
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}

