using CMS.Entities;

namespace CMS.Services
{
    public interface IWebsiteService
    {
        Task<int?> GetWebsiteIdByWebPageIdAsync(int webpageId);
    }
}
