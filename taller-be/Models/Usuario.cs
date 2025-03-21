﻿using System;
using System.Collections.Generic;

namespace taller_be.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
