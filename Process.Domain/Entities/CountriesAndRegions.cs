namespace Process.Domain.Entities
{
    public class CountriesAndRegions
    {
        public int CountryId { get; set; }
        public string Flag { get; set; } = string.Empty;
        public string NameESP { get; set; } = string.Empty;
        public string Indicative { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public bool FrecuentCountry { get; set; }
    }

}
