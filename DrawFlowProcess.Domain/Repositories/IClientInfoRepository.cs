namespace DrawFlowProcess.Domain.Repositories
{
    public interface IClientInfoRepository
    {
        string GetBrowser();
        string GetOS();
        string GetDevice();
    }
}
