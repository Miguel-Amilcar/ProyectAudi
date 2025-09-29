using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("TIPO_OPERACION", Name = "UQ__OPERACIO__8BA7B5F458B30FB2", IsUnique = true)]
public partial class OPERACION
{
    [Key]
    public int OPERACION_ID { get; set; }

    [StringLength(20)]
    public string TIPO_OPERACION { get; set; } = null!;

    [StringLength(255)]
    public string? DESCRIPCION { get; set; }

    [InverseProperty("OPERACION")]
    public virtual ICollection<BITACORA_ENCABEZADO> BITACORA_ENCABEZADO { get; set; } = new List<BITACORA_ENCABEZADO>();
}
