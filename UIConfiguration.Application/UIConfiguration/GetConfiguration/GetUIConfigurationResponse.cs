namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetUIConfigurationResponse
    {
        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
