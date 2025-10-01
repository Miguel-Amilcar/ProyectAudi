using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("USUARIO_NOMBRE", Name = "UQ__CREDENCI__23CA049BBCCAEC42", IsUnique = true)]
public partial class CREDENCIAL
{
    [Key]
    public int CREDENCIAL_ID { get; set; }

    [Required, StringLength(50)]
    [Unicode(false)]
    public string USUARIO_NOMBRE { get; set; } = null!;

    [Required, MaxLength(128)]
    public byte[] USUARIO_CONTRASENA_HASH { get; set; } = null!;

    [Required, MaxLength(64)]
    public byte[] USUARIO_SALT { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? PASSWORD_ULTIMO_CAMBIO { get; set; }

    public bool USUARIO_CAMBIOINICIAL { get; set; }

    public int INTENTOS_FALLIDOS { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BLOQUEADO_HASTA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FECHA_ULTIMO_INTENTO { get; set; }

    public bool MFA_ENABLED { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string? MFA_SECRET_BASE32 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? MFA_ULTIMO_USO { get; set; }

    [Required]
    public int USUARIO_ID { get; set; }

    [ForeignKey(nameof(USUARIO_ID))]
    [InverseProperty(nameof(USUARIO.CREDENCIAL))]
    public virtual USUARIO USUARIO { get; set; } = null!;
}
