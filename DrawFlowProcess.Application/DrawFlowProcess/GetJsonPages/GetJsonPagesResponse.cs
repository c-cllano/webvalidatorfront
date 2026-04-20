namespace DrawFlowProcess.Application.DrawFlowProcess.GetJsonPages
{
    public class GetJsonPagesResponse
    {
        public int CountPages { get; set; }
        public Dictionary<string, object> Pages { get; set; } = default!;
    }
}
