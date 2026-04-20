namespace Process.Application.UseCases.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PlatformConnectionAttribute(string platformConnectionType) : Attribute
    {
        public string PlatformConnectionType { get; } = platformConnectionType;
    }
}
