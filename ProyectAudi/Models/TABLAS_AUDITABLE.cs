using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("NOMBRE_TABLA", Name = "UQ__TABLAS_A__B050BA244E33F7AD", IsUnique = true)]
public partial class TABLAS_AUDITABLE
{
    [Key]
    public int TABLA_ID { get; set; }

    [StringLength(100)]
    public string NOMBRE_TABLA { get; set; } = null!;

    [StringLength(255)]
    public string? DESCRIPCION { get; set; }

    public bool ACTIVO { get; set; }

}
