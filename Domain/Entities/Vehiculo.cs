using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_transporte.Domain.Entities
{
    [Table("TRANS_vehiculo")]
    public class Vehiculo
    {
        [Key]
        [Column("id_vehiculo")]
        public int Id { get; set; }

        [Column("vc_placa")]
        public string Placa { get; set; } = string.Empty;

        [Column("i_tipo_vehiculo")]
        public int TipoVehiculo { get; set; }

        [Column("i_capacidad")]
        public int Capacidad { get; set; }

        [Column("i_cantidad_minima")]
        public int? CantidadMinima { get; set; }

        [Column("i_valor_asiento")]
        public decimal? ValorAsiento { get; set; }

        [Column("d_fv_soat")]
        public DateTime? FechaVencSoat { get; set; }

        [Column("d_fv_rev_tecnica")]
        public DateTime? FechaVencRevTecnica { get; set; }

        [Column("d_fv_rev_permiso_trans")]
        public DateTime? FechaVencPermisoTrans { get; set; }

        [Column("id_transportista")]
        public int IdTransportista { get; set; }

        [Column("i_estado_vehiculo")]
        public int EstadoVehiculo { get; set; }
    }
}
