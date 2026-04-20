namespace Process.Domain.Parameters.Document
{
    public class DatosBioDocu
    {
        /// <summary>
        /// Guid del proceso convenio
        /// </summary>
        public Guid GuidProcesoConvenio { get; set; }
        /// <summary>
        /// Guid del ciudadano al que se le desea guardar la biometría
        /// </summary>
        public Guid GuidCiu { get; set; }
        /// <summary>
        /// Tipo documento
        /// </summary>
        public string DatosAdi { get; set; } = string.Empty;
        /// <summary>
        /// Documento Anverso
        /// </summary>
        public ObjBio Anverso { get; set; } = default!;
        /// <summary>
        /// Documento Reverso
        /// </summary>
        public ObjBio Reverso { get; set; } = default!;
        /// <summary>
        /// Asesor
        /// </summary>
        public string Usuario { get; set; } = string.Empty;
        /// <summary>
        /// Trazabilidad usuario solicitante
        /// </summary>
        public Trazabilidad Trazabilidad { get; set; } = default!;
    }

    public class ObjBio
    {
        /// <summary>
        /// Imagen de la biometria en base64
        /// </summary>
        public string Valor { get; set; } = string.Empty;
        /// <summary>
        /// Formato de la imagen
        /// </summary>
        public string Formato { get; set; } = string.Empty;
    }


    public class Trazabilidad
    {
        public long Tiempo { get; set; }
        public string Ip { get; set; }
        public string Resolucion { get; set; } = string.Empty;
        public string Navegador { get; set; } = string.Empty;
        public string VersionNavegador { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }
}
