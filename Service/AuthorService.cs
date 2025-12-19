using GraphQL_Learning.Exceptions;
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
            => await _context.Authors.Include(a => a.Books)
                .Where(a => a.Id == id).FirstOrDefaultAsync();

        public IQueryable<Author> GetAuthors()
            => _context.Authors.Include(a => a.Books);

        public async Task<Author> AddAuthorAsync(AddAuthorInput input)
        {
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name == input.Name);
            if (existingAuthor != null)
            {
                throw new DuplicateEntityException("An Author witn the same name already exists.");
            }

            Author author = new()
            {
                Name = input.Name
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAuthorAsync(UpdateAuthorInput input)
        {
            Author? author = await GetAuthorAsync(input.Id) ?? throw new NotFoundException("Author not found");

            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name == input.Name && a.Id != input.Id);
            if (existingAuthor != null && existingAuthor.Name == input.Name)
            {
                throw new DuplicateEntityException($"Author {existingAuthor.Name} already exists");
            }

            if (!string.IsNullOrEmpty(input.Name) && author.Name != input.Name)
            {
                author.Name = input.Name;
                author.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return author;
            }
            return null;
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
