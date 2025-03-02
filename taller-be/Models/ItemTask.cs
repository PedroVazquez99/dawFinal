using System;
using System.Collections.Generic;

namespace taller_be.Models
{
    public partial class ItemTask
    {
        public decimal Id { get; set; }
        public string Texto { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public DateTime? Caduca { get; set; }
        public string Visible { get; set; } = null!;
        public string Terminada { get; set; } = null!;
        public decimal? Lista { get; set; }

        public virtual TaskList? ListaNavigation { get; set; }
    }
}
