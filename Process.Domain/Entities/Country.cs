namespace Process.Domain.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string? Name { get; set; }
        public string? Indicative { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
