namespace UITemplate.Domain.Repositories
{
    public interface IUITemplateRepository
    {
        public Task<object> GetUItemplate(string clientId);
    }
}
