using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

public partial class RECUPERACION_CONTRASENA
{
    [Key]
    public int RECUPERACION_ID { get; set; }

    public int CREDENCIAL_ID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TOKEN { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FECHA_SOLICITUD { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FECHA_EXPIRACION { get; set; }

    public bool USADO { get; set; }

    [ForeignKey("CREDENCIAL_ID")]
    [InverseProperty("RECUPERACION_CONTRASENA")]
    public virtual CREDENCIAL CREDENCIAL { get; set; } = null!;
}
