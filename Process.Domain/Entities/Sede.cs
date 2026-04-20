namespace Process.Domain.Entities
{
    public class Sede
    {
        public int SedeId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int ConvenioId { get; set; }
        public string Area { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string IdComercio { get; set; } = string.Empty;
        public string IdTermial { get; set; } = string.Empty;

    }
}
