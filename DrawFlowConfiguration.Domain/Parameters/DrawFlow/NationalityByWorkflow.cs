namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow
{
    public class NationalityByWorkflow
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public int WorkflowsNacionalidadesPermitidasId { get; set; }
        public int CountryId { get; set; }
        public string? NameCountry { get; set; }
        public string Indicative { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string Flag { get; set; } = string.Empty;
        public bool frecuentCountry { get; set; }
        public string? NameRegion { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

 
        public string? regionName { get; set; }
        public string? nameESP { get; set; }


    }
}
