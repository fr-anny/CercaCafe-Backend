using CercaCafe.DAL.Models;
using System;
using System.Collections.Generic;

namespace CercaCafe.DAL.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdCafeteria { get; set; }

    public string Dia { get; set; } = null!;

    public TimeOnly HoraApertura { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public virtual Cafeteria IdCafeteriaNavigation { get; set; } = null!;
}
