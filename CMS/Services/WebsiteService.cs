using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IGetCurrentUserService _currentUserService;

        public WebsiteService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IGetCurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        public async Task<int?> GetWebsiteIdByWebPageIdAsync(int webpageId)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            // Assuming there is a WebPages DbSet and WebsiteId foreign key in the WebPage entity
            var webpage = await dbContext.WebPages
                .Where(wp => wp.WebPageId == webpageId)
                .Select(wp => wp.WebSiteId)
                .FirstOrDefaultAsync();

            return webpage;
        }
    }
}
