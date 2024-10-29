using CMS.Entities;

namespace CMS.Services
{
    public interface IContentService
    {
        Task<Content?> GetContentAsync(int contentId);
        Task<List<Content>> GetAllContentsAsync();
        Task<List<Content>> GetContentsByWebPageIdAsync(int webPageId);
        Task SaveContentAsync(Content content);
        Task UpdateContentAsync(Content content);
        Task UpdateContentOrderAsync(List<Content> reorderedContents);
    }
}
