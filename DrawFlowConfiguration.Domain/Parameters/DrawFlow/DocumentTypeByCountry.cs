namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class DocumentTypeByCountry
    {
        public int DocumentTypeByCountryId { get; set; }
        public int CountryId { get; set; }
        public string Flag { get; set; } = string.Empty;
        public string NameCountry { get; set; } = string.Empty;
        public string NameESP { get; set; } = string.Empty;
        public string Indicative { get; set; } = string.Empty;
        public bool FrecuentCountry { get; set; }
        public int DocumentTypeId { get; set; }
        public string CodeDocumentType { get; set; } = string.Empty;
        public string NameDocumentType { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string NameRegion { get; set; } = string.Empty;
        public bool Active { get; set; }
        public int Length { get; set; }
        public string RegularExpression { get; set; } = string.Empty;
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
    }
}
