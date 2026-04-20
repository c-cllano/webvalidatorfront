namespace Process.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuditLogAttribute(string action = "") : Attribute
    {
        public string Action { get; } = action;
    }
}
