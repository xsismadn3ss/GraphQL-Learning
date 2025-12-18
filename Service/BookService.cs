using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_Learning.Service
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetBookAsync(int id)
        {
            return _context.Books.Include(b => b.Author)
                .Where(b => b.Id == id).FirstOrDefault();
        }

        public IQueryable<Book> GetBooks()
        {
            return _context.Books.Include(a => a.Author);
        }

        public async Task<Book> AddBookAsync(AddBookInput input)
        {
            try
            {
                Book book = new()
                {
                    Title = input.Title,
                    PublishedOn = input.PublishedOn,
                    AuthorId = input.AuthorId
                };
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return book;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true ||
                                                ex.InnerException?.Message.Contains("IX_") == true)
            {
                throw new InvalidOperationException("UNIQUE_CONSTRAINT_ERROR");
            }
        }

        public async Task<Book?> UpdateBookAsync(UpdateBookInput input)
        {
            Book? book = await _context.Books.FindAsync(input.Id);

            if (book == null)
                return null;

            try
            {
                bool updated = false;

                if (!string.IsNullOrEmpty(input.Title))
                {
                    book.Title = book.Title;
                    updated = true;
                }
                if (input.AuthorId.HasValue)
                {
                    book.AuthorId = input.AuthorId.Value;
                    updated = true;
                }
                if (input.PublishedOn.HasValue)
                {
                    book.PublishedOn = input.PublishedOn.Value;
                    updated = true;
                }
                if (updated)
                {
                    book.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                return book;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true ||
                                                ex.InnerException?.Message.Contains("IX_") == true)
            {
                throw new InvalidOperationException("UNIQUE_CONSTRAINT_ERROR");
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            Book? book = await _context.Books.FindAsync(id);
            if (book == null) return false;
            book.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
