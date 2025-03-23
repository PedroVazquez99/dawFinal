using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_be.Models
{
    [Table("Servicio")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(250)]
        [Column("nombre", TypeName = "varchar(250)")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250)]
        [Column("descripcion", TypeName = "varchar(250)")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column("precio", TypeName = "decimal(5, 2)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        public TimeSpan Duracion { get; set; }

    }
}

