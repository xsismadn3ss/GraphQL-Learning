using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_Learning.Service
{
    public class AuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Author?> GetAuthorAsync(int id)
        {
            return await _context.Authors.Include(a => a.Books)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Author> GetAuthors()
        {
            return _context.Authors.Include(a => a.Books);
        }

        public async Task<Author> AddAuthorAsync(AddAuthorInput input)
        {
            try
            {
                Author author = new()
                {
                    Name = input.Name
                };
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();
                return author;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true ||
                                              ex.InnerException?.Message.Contains("IX_") == true)
            {
                throw new InvalidOperationException("UNIQUE_CONTRAINT_ERROR");
            }
        }

        public async Task<Author?> UpdateAuthorAsync(UpdateAuthorInput input)
        {
            Author? author = await _context.Authors.FindAsync(input.id);

            if (author == null) return null;
            try
            {
                if (!string.IsNullOrEmpty(author.Name))
                {
                    author.Name = input.Name;
                    author.UpdatedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
                return author;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true ||
                                              ex.InnerException?.Message.Contains("IX_") == true)
            {
                throw new InvalidOperationException("UNIQUE_CONTRAINT_ERROR");
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            Author? author = await _context.Authors.FindAsync(id);
            if (author == null) return false;
            author.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
