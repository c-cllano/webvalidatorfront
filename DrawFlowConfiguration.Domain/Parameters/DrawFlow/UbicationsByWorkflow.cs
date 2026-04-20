namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class UbicationsByWorkflow
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int WorkflowsUbicacionesPermitidasId { get; set; }
        public int CountryId { get; set; }
        public string? NameCountry { get; set; }
        public string Indicative { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string Flag { get; set; } = string.Empty;
        public bool frecuentCountry { get; set; }
        public string? NameRegion { get; set; }
        public bool Active { get; set; }
        public bool? IsDeleted { get; set; }

        
        public string? nameESP  { get; set; }
        public string? regionName { get; set; }
    }
}
