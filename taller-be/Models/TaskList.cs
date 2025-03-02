using System;
using System.Collections.Generic;

namespace taller_be.Models
{
    public partial class TaskList
    {
        public TaskList()
        {
            ItemTasks = new HashSet<ItemTask>();
        }

        public decimal Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string Visible { get; set; } = null!;
        public string Color { get; set; } = null!;

        public virtual ICollection<ItemTask> ItemTasks { get; set; }
    }
}
