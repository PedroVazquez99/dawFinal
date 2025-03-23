using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace taller_be.Models
{
    public partial class TaskList
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Color { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(1)]
        public string Visible { get; set; } = null!;

        [Required]
        public int UsuarioId { get; set; } 

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        [Required]
        public int ServicioId { get; set; } 

        [ForeignKey("ServicioId")]
        public virtual Servicio? Servicio { get; set; }

    }
}

