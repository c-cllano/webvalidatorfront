namespace Process.API.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class AccessUriPermissionAttribute(string action = "") : Attribute
    {
        public string Action { get; } = action;
    }
}
