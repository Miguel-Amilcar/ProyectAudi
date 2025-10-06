using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models
{
    public partial class TOKEN_RECUPERACION
    {
        [Key]
        public int TOKEN_ID { get; set; }

        [Required]
        public int USUARIO_ID { get; set; }

        [Required, StringLength(6)]
        [Unicode(false)]
        public string TOKEN_VALOR { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FECHA_CREACION { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FECHA_EXPIRACION { get; set; }

        public bool USADO { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? FECHA_USO { get; set; }

        [StringLength(45)]
        [Unicode(false)]
        public string? IP_USO { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string? USER_AGENT { get; set; }

        [Required, StringLength(50)]
        [Unicode(false)]
        public string CREADO_POR { get; set; } = null!;

        [StringLength(50)]
        [Unicode(false)]
        public string? CONSUMIDO_POR { get; set; }

        [ForeignKey(nameof(USUARIO_ID))]
        [InverseProperty(nameof(USUARIO.TOKEN_RECUPERACION))]
        public virtual USUARIO USUARIO { get; set; } = null!;
    }

}
