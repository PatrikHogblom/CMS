using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class ContentService : IContentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IGetCurrentUserService _currentUserService;

        public ContentService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IGetCurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        // Method to retrieve content by ID
        public async Task<Content?> GetContentAsync(int contentId)
        {
            await using var context = _dbContextFactory.CreateDbContext();
            return await context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);
        }

        public async Task<List<Content>> GetAllContentsAsync()
        {
            await using var context = _dbContextFactory.CreateDbContext();
            return await context.Contents.ToListAsync();
        }

        public async Task<List<Content>> GetContentsByWebPageIdAsync(int webPageId)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            // Fetching the contents for the given WebPageId and ordering them
            return await context.Contents
                .Where(c => c.WebPageId == webPageId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        // Method to save new content to the database
        public async Task SaveContentAsync(Content content)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            // Ensure WebPageId exists in the WebPages table
            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == content.WebPageId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {content.WebPageId} does not exist.");
            }

            // Determine the current max Order for this WebPageId
            var maxOrder = await context.Contents
                .Where(c => c.WebPageId == content.WebPageId)
                .MaxAsync(c => (int?)c.Order) ?? 0;

            // Set the Order of the new content
            content.Order = maxOrder + 1;

            // Add the content with the new Order value
            context.Contents.Add(content);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }


        // Method to update existing content in the database
        public async Task UpdateContentAsync(Content content)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var existingContent = await context.Contents.FirstOrDefaultAsync(c => c.ContentId == content.ContentId);
            if (existingContent == null)
            {
                throw new InvalidOperationException($"Content with ID {content.ContentId} does not exist.");
            }

            existingContent.ContentName = content.ContentName;
            existingContent.ContentJson = content.ContentJson;
            existingContent.LastUpdated = DateOnly.FromDateTime(DateTime.Now);

            context.Contents.Update(existingContent);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }
        public async Task UpdateContentOrderAsync(List<Content> reorderedContents)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            for (int i = 0; i < reorderedContents.Count; i++)
            {
                reorderedContents[i].Order = i + 1;  // Set the new order, starting from 1
                context.Contents.Update(reorderedContents[i]);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error updating order: {ex.InnerException?.Message}");
                throw;
            }
        }

    }

}
