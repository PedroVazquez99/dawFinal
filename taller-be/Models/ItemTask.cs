using System;
using System.Collections.Generic;

namespace taller_be.Models
{
    public partial class ItemTask
    {
        public int Id { get; set; }
        public DateTime? Caduca { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Lista { get; set; }
        public string Terminada { get; set; } = null!;
        public string Texto { get; set; } = null!;
        public string Visible { get; set; } = null!;
    }
}
