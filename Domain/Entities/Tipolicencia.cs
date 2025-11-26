using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_transporte.Domain.Entities
{
    [Table("TRANS_tipo_licencia")]
    public class Tipolicencia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("vc_nombre")]
        public string? VcNombre { get; set; }

        [Column("vc_descripcion")]
        public string? VcDescripcion { get; set; }

        [Column("b_estado")]
        public bool? BEstado { get; set; }

        [Column("user_cr")]
        public string? UserCr { get; set; }

        [Column("date_cr")]
        public DateTime? DateCr { get; set; }
    }
}
