using GraphQL_Learning.Exceptions;
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
            => _context.Books.Include(b => b.Author)
                .Where(b => b.Id == id).FirstOrDefault();

        public IQueryable<Book> GetBooks()
            => _context.Books.Include(a => a.Author);

        public async Task<Book> AddBookAsync(AddBookInput input)
        {
            // validar si ya existe un libro con el mismo titulo
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == input.Title);
            if (existingBook != null) throw new DuplicateEntityException("A book with the same title already exists.");

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

        public async Task<Book?> UpdateBookAsync(UpdateBookInput input)
        {
            Book? book = await GetBookAsync(input.Id) ?? throw new NotFoundException("Book not found");

            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == input.Title && b.Id != input.Id);
            if (existingBook != null && existingBook.Title != input.Title)
            {
                throw new DuplicateEntityException($"Book {existingBook.Title} already exists");
            }

            bool updated = false;

            if (!string.IsNullOrEmpty(input.Title) && input.Title != book.Title)
            {
                book.Title = input.Title;
                updated = true;
            }
            if (input.AuthorId.HasValue && input.AuthorId.Value != book.AuthorId)
            {
                book.AuthorId = input.AuthorId.Value;
                updated = true;
            }
            if (input.PublishedOn.HasValue && input.PublishedOn != book.PublishedOn)
            {
                book.PublishedOn = input.PublishedOn.Value;
                updated = true;
            }
            if (updated)
            {
                book.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return book;
            }
            return null;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            Book? book = await GetBookAsync(id);
            if (book == null) return false;
            book.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
