namespace DrawFlowProcess.Domain.Domain
{
    public class JsonPages
    {
        public int CountPages { get; set; }
        public Dictionary<string, object> Pages { get; set; } = default!;
    }
}
