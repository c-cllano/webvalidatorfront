namespace Process.Domain.ValueObjects
{
    public class AtdpVersionFile(
        int versionId, 
        string version, 
        string fileContent, 
        string? sas
    )
    {
        public int AtdpVersionID { get; } = versionId;
        public string? Version { get; } = version;
        public string FileContent { get; } = fileContent;
        public string sas { get; } = sas!;
    }

}
