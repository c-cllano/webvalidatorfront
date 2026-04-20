namespace Process.Application.UseCases.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AntiSpoofingAttribute(string type) : Attribute
    {
        public string Type { get; } = type;
    }
}
