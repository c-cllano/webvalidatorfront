
namespace Process.Domain.Entities
{
    public class AgreementProcess
    {
        public long ProcesoConvenioId { get; set; }
        public long? ConvenioId { get; set; }

        public bool? Finalizado { get; set; }
        public bool? Cancelado { get; set; }
        public DateTime? CreatedAt { get; set; }

        public long? AsesorId { get; set; }
        public long? SedeId { get; set; }

        public int? RevisionForence { get; set; }
        public long? TipoDocumentoId { get; set; }

        public Guid? AppFrontGuid { get; set; }
        public long? CiudadanoId { get; set; }

        public bool? EjecutarEnMovil { get; set; }

        public int? EstadoProceso { get; set; }
        public string InfCandidato { get; set; } = string.Empty;

        public DateTime? FechaFinalizacion { get; set; }

        public bool? Validacion { get; set; }
    }
}
